using System.Net;
using System.Net.Http.Json;

namespace Company.WebAPI.Clients.Base
{
    public abstract class BaseClient : IDisposable
    {
        protected HttpClient Http { get; }

        protected string Address { get; }

        protected BaseClient(HttpClient Client, string Address)
        {
            Http = Client;
            this.Address = Address;
        }

        protected T? Get<T>(string url) => GetAsync<T>(url).Result;
        protected async Task<T?> GetAsync<T>(string url, CancellationToken cancel = default)
        {
            var response = await Http.GetAsync(url, cancel).ConfigureAwait(false);
            if (response.StatusCode == HttpStatusCode.NoContent) return default(T);
            return await response
                .EnsureSuccessStatusCode()
                .Content
                .ReadFromJsonAsync<T>() //Десериализация из JSON в тип T
                .ConfigureAwait(false);
        }

        protected HttpResponseMessage? Post<T>(string url, T value) => PostAsync<T>(url, value).Result;
        protected async Task<HttpResponseMessage?> PostAsync<T>(string url, T value, CancellationToken cancel = default)
        {
            var response = await Http.PostAsJsonAsync(url, value, cancel).ConfigureAwait(false);
            return response.EnsureSuccessStatusCode();
        }

        protected HttpResponseMessage? Put<T>(string url, T value) => PutAsync<T>(url, value).Result;
        protected async Task<HttpResponseMessage?> PutAsync<T>(string url, T value, CancellationToken cancel = default)
        {
            var response = await Http.PutAsJsonAsync(url, value, cancel).ConfigureAwait(false);
            return response.EnsureSuccessStatusCode();
        }

        protected HttpResponseMessage? Delete(string url) => DeleteAsync(url).Result;
        protected async Task<HttpResponseMessage?> DeleteAsync(string url, CancellationToken cancel = default)
        {
            var response = await Http.DeleteAsync(url, cancel).ConfigureAwait(false);
            return response;
        }

        public void Dispose() => Dispose(true);

        protected bool _Disposed;
        protected virtual void Dispose(bool disposing)
        {
            if (_Disposed) return;
            _Disposed = true;

            if (disposing)
            {
                //освобождаем управляемые ресурсы - обычные объекты с интерфейсом IDisposable
                //Http.Dispose(); // не должны вызывать Dispose(), потому что не мы его создавали
            }

            //освобождаем неуправляемые ресурсы: COM-объекты например
        }
    }
}
