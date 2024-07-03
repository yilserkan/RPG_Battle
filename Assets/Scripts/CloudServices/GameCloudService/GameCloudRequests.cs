using RPGGame.Game;
using System;
using System.Threading.Tasks;
using UnityEngine;

namespace RPGGame.CloudServices
{
    public static class GameCloudRequests
    {

        private static IGameCloudService gameCloudService = new MockGameCloudService();

        public static async Task<CreateLevelDataResponse> CreateLevelData(string[] selectedHeroIds)
        {
            try
            {
                var json = await gameCloudService.CreateLevelData(selectedHeroIds);
                var response = JsonUtility.FromJson<CreateLevelDataResponse>(json);
                response.IsSuccessfull = true;
                return response;
            }
            catch(Exception e)
            {
                Debug.LogError("Request Error : " + e.Message);
                var response = new CreateLevelDataResponse() 
                { IsSuccessfull = false, 
                    LevelData = new LevelData(new GameHeroData[0], new GameHeroData[0])
                };
                return response;
            }
        }

        public static async Task<AttackEnemyHeroResponse> AttackEnemyPlayer(string attackerId)
        {
            try
            {
                var json = await gameCloudService.AttackEnemyPlayer(attackerId);
                var response = JsonUtility.FromJson<AttackEnemyHeroResponse>(json);
                response.IsSuccessfull = true;
                return response;
            }
            catch (Exception e)
            {
                Debug.LogError("Request Error : " + e.Message);
                var response = new AttackEnemyHeroResponse()
                {
                    IsSuccessfull = false,
                    Damage = 0,
                    EnemyId = ""
                };
                return response;
            }
        }

        public static async Task<SimulateEnemyAttackResponse> SimulateEnemyAttack()
        {
            try
            {
                var json = await gameCloudService.SimulateEnemyAttack();
                var response = JsonUtility.FromJson<SimulateEnemyAttackResponse>(json);
                response.IsSuccessfull = true;
                return response;
            }
            catch (Exception e)
            {
                Debug.LogError("Request Error : " + e.Message);
                var response = new SimulateEnemyAttackResponse()
                {
                    IsSuccessfull = false,
                    Damage = 0,
                    ReceiverHeroID = ""
                };
                return response;
            }
        }
    }


    [Serializable]
    public class BaseResponse
    {
        public bool IsSuccessfull;
    }

    [Serializable]
    public class CreateLevelDataResponse : BaseResponse
    {
        public LevelData LevelData;
    }

    [Serializable]
    public class AttackEnemyHeroResponse : BaseResponse
    {
        public string EnemyId;
        public float Damage;
    }

    [Serializable]
    public class SimulateEnemyAttackResponse : BaseResponse
    {
        public string AttackerHeroID;
        public string ReceiverHeroID;
        public float Damage;
    }
}
