using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;

namespace RPGGame.Extensions
{
    public static class AddressableExtensions
    {
        public static void InstantiateAsyncAction<T>(this AssetReferenceT<T> reference, Transform parent = null, Action<GameObject> onInstantiated = null, Action onFailed = null) where T : Object
        {
            reference.InstantiateAsync(parent).Completed += (operationHandle) =>
            {
                if (operationHandle.Status == AsyncOperationStatus.Succeeded)
                {
                    onInstantiated?.Invoke(operationHandle.Result);
                }
                else
                {
                    onFailed?.Invoke();
                }
            };
        }

        public static void LoadAddressableAction<T>(this AssetReferenceT<T> reference, Action<T> onLoaded, Action onFailed = null) where T : Object
        {
            reference.LoadAssetAsync<T>().Completed += (operationHandle) =>
            {
                if (operationHandle.Status == AsyncOperationStatus.Succeeded)
                {
                    onLoaded?.Invoke(operationHandle.Result);
                }
                else
                {
                    onFailed?.Invoke();
                }
            };
        }

        public static async Task<GameObject> InstantiateAddressableAsync<T>(this AssetReferenceT<T> reference, Transform parent = null) where T : Object
        {
            var gameObject = await reference.InstantiateAsync(parent).Task;
            return gameObject;
        }

        public static async Task<AsyncOperationHandle<T>> LoadAddressableAsync<T>(this AssetReferenceT<T> reference) where T : Object
        {
            var handle = reference.LoadAssetAsync<T>();
            await handle.Task;
            return handle;
        }
    }
}
