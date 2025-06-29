namespace Movies.Api.Auth
{
    public static class IdentityExtensions
    {
        public static Guid? GetUserId(this HttpContext context)
        {
            var userId = context.User.Claims.FirstOrDefault(c => c.Type == "userid");

            if (Guid.TryParse(userId?.Value, out Guid parsedId))
            {
                return parsedId;
            }
            return null;
        }
    }
}
