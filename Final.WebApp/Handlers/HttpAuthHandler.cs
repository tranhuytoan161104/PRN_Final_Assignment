using Microsoft.AspNetCore.Authentication;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace Final.WebApp.Handlers
{

    // Handler này sẽ tự động thêm token vào header của mỗi request
    public class HttpAuthHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HttpAuthHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext?.User.Identity?.IsAuthenticated ?? false)
            {
                // Lấy token đã lưu trong cookie claims
                var token = httpContext.User.FindFirstValue("access_token");

                if (!string.IsNullOrEmpty(token))
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }
            }
            return await base.SendAsync(request, cancellationToken);
        }
    }
}