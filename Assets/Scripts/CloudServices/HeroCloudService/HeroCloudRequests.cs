using RPGGame.Hero;
using System;
using System.Threading.Tasks;
using UnityEngine;

namespace RPGGame.CloudServices
{
    public static class HeroCloudRequests
    {
        private static IHeroCloudService heroCloudService = new MockHeroCloudSerivce();
        
        public static async Task<LoadHeroDataResponse> LoadHeroData()
        {
            try
            {
                var json = await heroCloudService.LoadHeroData();
                var response = JsonUtility.FromJson<LoadHeroDataResponse>(json);
                response.IsSuccessfull = true;
                return response;
            }
            catch (Exception e)
            {
                Debug.LogError("Request Error : " + e.Message);
                var response = new LoadHeroDataResponse()
                {
                    IsSuccessfull = false,
                    HeroDatas = new HeroData[0]
                };
                return response;
            }
        }

        public static async Task<BaseResponse> AddRandomHeroToPlayer()
        {
            try
            {
                var json = await heroCloudService.AddRandomHeroToPlayer();
                var response = JsonUtility.FromJson<BaseResponse>(json);
                response.IsSuccessfull = true;
                return response;
            }
            catch (Exception e)
            {
                Debug.LogError("Request Error : " + e.Message);
                var response = new BaseResponse()
                {
                    IsSuccessfull = false
                };
                return response;
            }
        }

        public static async Task<BaseResponse> IncreaseHeroExperiences(string heroIdsRequest)
        {
            try
            {
                var json = await heroCloudService.IncreaseHeroExperiences(heroIdsRequest);
                var response = JsonUtility.FromJson<BaseResponse>(json);
                response.IsSuccessfull = true;
                return response;
            }
            catch (Exception e)
            {
                Debug.LogError("Request Error : " + e.Message);
                var response = new BaseResponse()
                {
                    IsSuccessfull = false
                };
                return response;
            }
        }
    }

    [Serializable]
    public class LoadHeroDataResponse : BaseResponse
    {
        public HeroData[] HeroDatas;
    }

    [Serializable]
    public class IncreaseHeroExperienceRequest
    {
        public string[] HeroIds;
    }
}

