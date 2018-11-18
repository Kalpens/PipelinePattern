using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PipelinePattern.Entity;

namespace PipelinePattern
{
    class LoadBalancedPipeline
    {
        /// <summary>
        /// Run a pipeline that uses a user-specified number of tasks for the filter stage.
        /// </summary>
        /// <param name="fileNames">List of image file names in source directory</param>
        /// <param name="sourceDir">Name of directory of source images</param>
        /// <param name="queueLength">Length of image queue</param>
        /// <param name="cts">Cancellation token</param>
        /// <param name="filterTaskCount">Number of filter tasks</param>
        public static void RunPipeline(IEnumerable<string> fileNames, string sourceDir, int queueLength,
            CancellationTokenSource token, int filterTaskCount)
        {
            using (CancellationTokenSource cts = CancellationTokenSource.CreateLinkedTokenSource(token.Token))
            {
                // Create data pipes 
                var originalImages = new BlockingCollection<ImageInfo>(queueLength);
                var thumbnailImages = new BlockingCollection<ImageInfo>(queueLength);
                var filteredImageMultiplexer =
                    new BlockingMultiplexer<ImageInfo>(info => info.SequenceNumber, 0, queueLength);
                var filteredImagesCollections = (BlockingCollection<ImageInfo>[]) Array.CreateInstance(
                    typeof(BlockingCollection<ImageInfo>), filterTaskCount);

                try
                {
                    const TaskCreationOptions options = TaskCreationOptions.LongRunning;
                    var factory = new TaskFactory(CancellationToken.None, options, TaskContinuationOptions.None,
                        TaskScheduler.Default);
                    Task[] tasks = (Task[]) Array.CreateInstance(typeof(Task), filterTaskCount + 3);
                    int taskId = 0;

                    tasks[taskId++] = factory.StartNew(() =>
                        PipelinePhases.LoadPipelinedImages(fileNames, sourceDir, originalImages, cts));

                    tasks[taskId++] = factory.StartNew(() =>
                        PipelinePhases.ScalePipelinedImages(originalImages, thumbnailImages, cts));

                    for (int i = 0; i < filterTaskCount; i++)
                    {
                        var tmp = i;
                        filteredImagesCollections[tmp] =
                            filteredImageMultiplexer.GetProducerQueue();
                        tasks[taskId++] = factory.StartNew(() =>
                            PipelinePhases.FilterPipelinedImages(thumbnailImages, filteredImagesCollections[tmp], cts));
                    }

                    tasks[taskId++] = factory.StartNew(() =>
                        PipelinePhases.SavePipelinedImages(filteredImageMultiplexer.GetConsumingEnumerable(), cts));

                    Task.WaitAll(tasks);
                }
                finally
                {
                    // there might be cleanup in the case of cancellation or an exception.
                    DisposeImagesInQueue(originalImages);
                    DisposeImagesInQueue(thumbnailImages);
                    foreach (var filteredImages in filteredImagesCollections)
                        DisposeImagesInQueue(filteredImages);
                    foreach (var info in filteredImageMultiplexer.GetCleanupEnumerable())
                        info.Dispose();
                }
            }
        }

        #region Cleanup methods used by error handling

            // Ensure that the queue contents is disposed. You could also implement this by 
            // subclassing BlockingCollection<> and providing an IDisposable implmentation.
            static void DisposeImagesInQueue(BlockingCollection<ImageInfo> queue)
            {
                if (queue != null)
                {
                    queue.CompleteAdding();
                    foreach (var info in queue)
                    {
                        info.Dispose();
                    }
                }
            }

            #endregion
        }
    }
