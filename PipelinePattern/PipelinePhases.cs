using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PipelinePattern.Entity;

namespace PipelinePattern
{
    public static class PipelinePhases
    {
        /// <summary>
        /// Image pipeline phase 1: Load images from disk and put them a queue.
        /// </summary>
        public static void LoadPipelinedImages(IEnumerable<string> fileNames, string sourceDir,
            BlockingCollection<ImageInfo> original, CancellationTokenSource cts)
        {
            int count = 0;
            var token = cts.Token;
            ImageInfo info = null;
            try
            {
                foreach (var fileName in fileNames)
                {
                    if (token.IsCancellationRequested)
                        break;
                    info = ImageLogic.LoadImage(fileName, sourceDir, count);
                    original.Add(info, token);
                    count += 1;
                    info = null;
                }
            }
            catch (Exception e)
            {
                // in case of exception, signal shutdown to other pipeline tasks
                cts.Cancel();
                if (!(e is OperationCanceledException))
                    throw;
            }
            finally
            {
                original.CompleteAdding();
                if (info != null) info.Dispose();
            }
        }

        /// <summary>
        /// Image pipeline phase 2: Scale to thumbnail size and render picture frame.
        /// </summary>
        public static void ScalePipelinedImages(BlockingCollection<ImageInfo> originalImages,
            BlockingCollection<ImageInfo> thumbnailImages, CancellationTokenSource cts)
        {
            var token = cts.Token;
            ImageInfo info = null;
            try
            {
                foreach (var infoTmp in originalImages.GetConsumingEnumerable())
                {
                    info = infoTmp;
                    if (token.IsCancellationRequested)
                        break;
                    ImageLogic.ScaleImage(info);
                    thumbnailImages.Add(info, token);
                    info = null;
                }
            }
            catch (Exception e)
            {
                cts.Cancel();
                if (!(e is OperationCanceledException))
                    throw;
            }
            finally
            {
                thumbnailImages.CompleteAdding();
                if (info != null) info.Dispose();
            }
        }

        /// <summary>
        /// Image pipeline phase 3: Filter images (give them a speckled appearance by adding Gaussian noise)
        /// </summary>
        public static void FilterPipelinedImages(
            BlockingCollection<ImageInfo> thumbnailImages,
            BlockingCollection<ImageInfo> filteredImages,
            CancellationTokenSource cts)
        {
            ImageInfo info = null;
            try
            {
                var token = cts.Token;
                foreach (ImageInfo infoTmp in
                    thumbnailImages.GetConsumingEnumerable())
                {
                    info = infoTmp;
                    if (token.IsCancellationRequested)
                        break;
                    ImageLogic.FilterImage(info);
                    filteredImages.Add(info, token);
                    info = null;
                }
            }
            catch (Exception e)
            {
                cts.Cancel();
                if (!(e is OperationCanceledException))
                    throw;
            }
            finally
            {
                filteredImages.CompleteAdding();
                if (info != null) info.Dispose();
            }
        }

        /// <summary>
        /// Image pipeline phase 4: Invoke the user-provided delegate (for example, to display the result in a UI)
        /// </summary>
        public static void SavePipelinedImages(IEnumerable<ImageInfo> filteredImages,
                                           CancellationTokenSource cts)
        {
            int count = 1;
            var token = cts.Token;
            ImageInfo info = null;
            try
            {
                foreach (ImageInfo infoTmp in filteredImages)
                {
                    info = infoTmp;
                    if (token.IsCancellationRequested)
                        break;
                    ImageLogic.SaveImage(info, count);

                    count = count + 1;
                    info = null;
                }
            }
            catch (Exception e)
            {
                cts.Cancel();
                if (!(e is OperationCanceledException))
                    throw;
            }
            finally
            {
                info?.Dispose();
            }
        }

    }
}
