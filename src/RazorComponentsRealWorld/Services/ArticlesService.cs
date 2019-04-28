﻿using System.Collections.Generic;
using System.Threading.Tasks;
using RazorComponentsRealWorld.Models;

namespace RazorComponentsRealWorld.Services
{
    public class ArticlesService
    {
        private IApiService api;

        public ArticlesService(IApiService _api)
        {
            api = _api;
        }

        public async Task<ApiResponse<ArticlesResponse>> QueryAsync(IDictionary<string, string> Params = null)
        {
            return await api.GetAsync<ArticlesResponse>($"/articles/", Params);
        }

        public async Task<ApiResponse<ArticlesResponse>> GetAllAsync()
        {
            return await QueryAsync();
        }

        public async Task<ApiResponse<ArticleResponse>> GetAsync(string Slug)
        {
            return await api.GetAsync<ArticleResponse>($"/articles/{Slug}");
        }

        public async Task<ApiResponse<ArticlesResponse>> GetFeedAsync()
        {
            return await api.GetAsync<ArticlesResponse>($"/articles/feed");
        }

        public async Task<ApiResponse<ArticlesResponse>> GetByAuthorAsync(string author)
        {
            return await QueryAsync(new Dictionary<string, string>
            {
                {  "author", author }
            });
        }

        public async Task<ApiResponse<ArticlesResponse>> GetByTagAsync(string tag)
        {
            return await QueryAsync(new Dictionary<string, string>
            {
                {  "tag", tag }
            });
        }

        public async Task<bool> DeleteAsync(string slug)
        {
            ApiResponse<ArticleModel> response = await api.DeleteAsync<ArticleModel>($"/articles/{slug}");
            return response?.HasSuccessStatusCode ?? false;
        }

        public async Task<ApiResponse<ArticleResponse>> SaveAsync(ArticleModel value)
        {
            string slug = value?.Slug ?? string.Empty;
            ApiResponse<ArticleResponse> response = null;

            if (string.IsNullOrWhiteSpace(slug))
            {
                response = await api.PostAsync<ArticleResponse>($"/articles/", new
                {
                    article = value
                });
            }
            else
            {
                response = await api.PutAsync<ArticleResponse>($"/articles/{slug}", new
                {
                    article = value
                });
            }

            return response;
        }

        public async Task<bool> FavoriteAsync(string Slug)
        {
            ApiResponse<ArticleModel> response = await api.PostAsync<ArticleModel>($"/articles/{Slug}/favorite");
            return response?.HasSuccessStatusCode ?? false;
        }

        public async Task<bool> UnfavoriteAsync(string Slug)
        {
            ApiResponse<ArticleModel> response = await api.DeleteAsync<ArticleModel>($"/articles/{Slug}/favorite");
            return response?.HasSuccessStatusCode ?? false;
        }
    }

    public class ArticlesResponse
    {
        public ArticleModel[] Articles { get; set; }
    }

    public class ArticleResponse
    {
        public ArticleModel Article { get; set; }
    }
}
