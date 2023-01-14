namespace SkrivLätt;

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
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
				fonts.AddFont("OpenDyslexic-Regular.otf", "OpenDyslexicRegular");
				fonts.AddFont("LibraSans.otf", "LibraSans");
                fonts.AddFont("LibraSerifModern-Regular.otf", "LibraSerifModernRegular");
            });

		return builder.Build();
	}
}
