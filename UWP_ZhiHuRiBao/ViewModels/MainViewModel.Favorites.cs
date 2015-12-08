using Brook.ZhiHuRiBao.Models;
using Brook.ZhiHuRiBao.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brook.ZhiHuRiBao.ViewModels
{
    public partial class MainViewModel
    {
        public int? FavoritesLastTime { get; set; } = null;

        public int FavoritesCount { get; set; } = 0;

        private async Task RequestFavorites(bool isLoadingMore)
        {
            Favorites favData = null;

            if (isLoadingMore)
            {
                if (!FavoritesLastTime.HasValue)
                    return;

                favData = await DataRequester.RequestFavorites(FavoritesLastTime.Value.ToString());
            }
            else
            {
                ResetStorys();
                favData = await DataRequester.RequestLatestFavorites();
                if (favData != null)
                {
                    CurrentStoryId = favData.stories.First().id.ToString();
                    FavoritesCount = favData.count;
                    CategoryName = string.Format(StringUtil.GetString("FavCategoryName"), FavoritesCount);
                }
            }

            if (favData == null)
                return;

            FavoritesLastTime = favData.last_time;

            StoryDataList.AddRange(favData.stories);
        }
    }
}
