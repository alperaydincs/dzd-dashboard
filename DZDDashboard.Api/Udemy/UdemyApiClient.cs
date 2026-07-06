using System.Net.Http.Headers;
using System.Text;
using Microsoft.Extensions.Options;

namespace DZDDashboard.Api.Udemy;

public interface IUdemyApiClient
{
    IAsyncEnumerable<UdemyActivityRecord> GetAllCourseActivityAsync(CancellationToken cancellationToken = default);
}

public class UdemyApiClient(HttpClient httpClient, IOptions<UdemyOptions> options) : IUdemyApiClient
{
    private readonly UdemyOptions _options = options.Value;

    public async IAsyncEnumerable<UdemyActivityRecord> GetAllCourseActivityAsync(
        [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var auth = Convert.ToBase64String(
            Encoding.ASCII.GetBytes($"{_options.ClientId}:{_options.ClientSecret}"));

        var url = $"organizations/{_options.AccountId}/analytics/user-course-activity/?page_size={_options.PageSize}";

        while (!string.IsNullOrEmpty(url))
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", auth);

            using var response = await httpClient.SendAsync(request, cancellationToken);
            response.EnsureSuccessStatusCode();

            var page = await response.Content.ReadFromJsonAsync<UdemyPagedResponse<UdemyActivityRecord>>(cancellationToken);
            if (page is null) yield break;

            foreach (var record in page.Results)
                yield return record;

            url = page.Next;
        }
    }
}
