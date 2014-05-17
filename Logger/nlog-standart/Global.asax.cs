
namespace Web
{
	public class Global : HttpApplication
	{
		void Application_Start(object sender, EventArgs e)
		{


			var logger = DependencyResolver.Current.GetService<Logger>();
			logger.WriteInfo("App start");
		}

		void Application_Error()
		{
			Exception error = Server.GetLastError();
			Server.ClearError();

			var logger = DependencyResolver.Current.GetService<Logger>();
			logger.WriteError(error);
		}
	}
}