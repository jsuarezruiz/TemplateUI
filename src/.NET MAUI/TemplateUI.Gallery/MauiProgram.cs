﻿using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;
using TemplateUI.Hosting;

namespace TemplateUI.Gallery
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                }).
                ConfigureTemplateUI();

            return builder.Build();
        }
    }
}