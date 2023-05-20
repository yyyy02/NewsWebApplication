namespace NewsWebApplication.Models
{
    public class LoginCheckr
    {
        private readonly RequestDelegate next;

        public LoginCheckr(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context) {
            if (context.Request.Path.StartsWithSegments("/Login/")) {
                await next(context);
                return;
            }

            string sessionId = context.Request.Cookies["sessionId"];
            if (sessionId == null)
            {
                context.Response.Redirect("/Login/");
            }
            else {
                await next(context);
            }
        }
    }
}
