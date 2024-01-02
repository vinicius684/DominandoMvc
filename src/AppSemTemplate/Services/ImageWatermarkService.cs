using AppSemTemplate.Data;
using Microsoft.EntityFrameworkCore;
using System.Drawing;

namespace AppSemTemplate.Services
{
    public class ImageWatermarkService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider; 
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string _watermarkPath;

        public ImageWatermarkService(IServiceProvider serviceProvider, IWebHostEnvironment webHostEnvironment)
        {
            _serviceProvider = serviceProvider;
            _webHostEnvironment = webHostEnvironment;
            _watermarkPath = Path.Combine(_webHostEnvironment.WebRootPath, "images/logo_watermark.png");

        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                    var unprocessedProducts = await context.Produto
                        .Where(p => !p.Processado)
                        .ToListAsync();

                    foreach (var product in unprocessedProducts)
                    {
                        try
                        {
                            var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", product.Imagem);
                            var newimagePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", "prod_" + product.Imagem);

                            using (Bitmap watermarkImage = new Bitmap(_watermarkPath))

                            using (Bitmap imagemOriginal = new Bitmap(imagePath))
                            {
                                using (Bitmap resizedImage = new Bitmap(381, 499))

                                using (var graphic = Graphics.FromImage(resizedImage))
                                {
                                    graphic.DrawImage(imagemOriginal, 0, 0, 381, 499);

                                    var point = new Point(resizedImage.Width - 180, resizedImage.Height - 80);

                                    graphic.DrawImage(watermarkImage, point);

                                    resizedImage.Save(newimagePath);
                                }
                            }

                            product.Processado = true;

                        }
                        catch (Exception ex)
                        { 
                        }
                    }

                    await context.SaveChangesAsync();
                }


                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
    }
}
