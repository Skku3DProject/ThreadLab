using Firebase;
using Firebase.Auth;
using Firebase.Firestore;
using System.Threading.Tasks;
using System;
using UnityEngine;

public abstract class FirebaseRepositoryBase
{
    //private static bool _isFirebaseReady = false;
    //private static Task _initTask;

    protected FirebaseAuth Auth => FirebaseAuth.DefaultInstance;
    protected FirebaseFirestore Firestore => FirebaseFirestore.DefaultInstance;

    //private async Task EnsureFirebaseInitialized()
    //{
    //    if (_isFirebaseReady) return;

    //    if (_initTask == null)
    //    {
    //        _initTask = FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
    //        {
    //            var result = task.Result;
    //            if (result == DependencyStatus.Available)
    //            {
    //                _isFirebaseReady = true;
    //                Debug.Log("✅ Firebase 종속성 확인 완료");
    //            }
    //            else
    //            {
    //                throw new Exception($"❌ Firebase 초기화 실패: {result}");
    //            }
    //        });
    //    }

    //    await _initTask;
    //}


    /// 반환값 있는 비동기 작업용
    protected async Task<T> ExecuteAsync<T>(Func<Task<T>> taskFunc, string context = "")
    {
        await FirebaseManager.Instance.InitTask;

        try
        {
            // 공통 로딩 UI 시작
            Debug.Log($"[Firebase] 시작: {context}");

            T result = await taskFunc.Invoke();

            // 로딩 UI 종료
            Debug.Log($"[Firebase] 성공: {context}");
            return result;
        }
        catch (Exception e)
        {
            Debug.LogError($"[Firebase] 실패: {context} - {e.Message}");
            throw; // 필요 시 사용자 정의 예외 래핑도 가능
        }
    }

    /// 반환값 없는 비동기 작업용
    protected async Task ExecuteAsync(Func<Task> taskFunc, string context = "")
    {
        await FirebaseManager.Instance.InitTask;

        try
        {
            Debug.Log($"[Firebase] 시작: {context}");

            await taskFunc.Invoke();

            Debug.Log($"[Firebase] 성공: {context}");
        }
        catch (Exception e)
        {
            Debug.LogError($"[Firebase] 실패: {context} - {e.Message}");
            throw;
        }
    }
}
