using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public enum PathLocation
{
    Resources,       // Config mặc định của game (Đọc bằng Resources.Load, ghi được trong Editor)
    PersistentData,  // Dữ liệu save game của người chơi (AppData / Documents)
    StreamingAssets  // File raw đi kèm game, có thể sửa ngoài bản build
}

public  class JsonDataService
{
    private static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
    {
        Formatting = Newtonsoft.Json.Formatting.Indented, // Xuất file đẹp, xuống dòng rõ ràng
        NullValueHandling = NullValueHandling.Ignore,
        Converters = { new StringEnumConverter() }, // Lưu Enum dạng chữ (Monster, Interest)
        MissingMemberHandling = MissingMemberHandling.Ignore

    };

    #region CORE LOAD & SAVE

    /// <summary>
    /// Nạp dữ liệu tổng quát (Class đơn, List, Dictionary...)
    /// </summary>
    public static T Load<T>(string relativePath, PathLocation location = PathLocation.Resources)
    {
        try
        {
            string jsonContent = string.Empty;

            if (location == PathLocation.Resources)
            {
                // Resources.Load không nhận đuôi .json
                string cleanPath = CleanResourcePath(relativePath);
                TextAsset asset = Resources.Load<TextAsset>(cleanPath);

                if (asset == null)
                {
                    Debug.LogWarning($"[Json] Không tìm thấy TextAsset tại: Resources/{cleanPath}");
                    return default;
                }

                jsonContent = asset.text;
                Resources.UnloadAsset(asset); // Giải phóng bộ nhớ RAM
            }
            else
            {
                string fullPath = GetFullPath(relativePath, location);
                if (!File.Exists(fullPath))
                {
                    Debug.LogWarning($"[Json] Không tìm thấy file: {fullPath}");
                    return default;
                }

                jsonContent = File.ReadAllText(fullPath);
            }

            return JsonConvert.DeserializeObject<T>(jsonContent, Settings);
        }
        catch (Exception ex)
        {
            Debug.LogError($"[Json] Lỗi khi đọc file '{relativePath}': {ex.Message}");
            return default;
        }
    }

    /// <summary>
    /// Ghi dữ liệu ra file
    /// </summary>
    public static bool Save<T>(T data, string relativePath, PathLocation location = PathLocation.Resources)
    {
        try
        {
            if (location == PathLocation.Resources && !Application.isEditor)
            {
                Debug.LogError("[Json] Chỉ có thể lưu vào Resources khi chạy trong Unity Editor!");
                return false;
            }

            string fullPath = GetFullPath(relativePath, location);
            string directory = Path.GetDirectoryName(fullPath);

            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            string json = JsonConvert.SerializeObject(data, Settings);
            File.WriteAllText(fullPath, json);

#if UNITY_EDITOR
            if (location == PathLocation.Resources || location == PathLocation.StreamingAssets)
            {
                UnityEditor.AssetDatabase.Refresh();
            }
#endif
            Debug.Log($"[Json] Lưu file thành công tới: {fullPath}");
            return true;
        }
        catch (Exception ex)
        {
            Debug.LogError($"[Json] Lỗi khi lưu file '{relativePath}': {ex.Message}");
            return false;
        }
    }

    #endregion

    #region SHORTCUT CHO DẠNG LIST

    /// <summary>
    /// Đọc trực tiếp ra danh sách List<T> (trả về danh sách rỗng nếu không có dữ liệu để tránh NullReference)
    /// </summary>
    public static List<T> LoadList<T>(string relativePath, PathLocation location = PathLocation.Resources)
    {
        var result = Load<List<T>>(relativePath, location);
        return result ?? new List<T>();
    }

    /// <summary>
    /// Lưu trực tiếp một List<T> ra file JSON dạng mảng [ ... ]
    /// </summary>
    public static bool SaveList<T>(List<T> listData, string relativePath, PathLocation location = PathLocation.Resources)
    {
        return Save(listData, relativePath, location);
    }

    #endregion

    #region PATH RESOLVER

    private static string CleanResourcePath(string path)
    {
        return path.EndsWith(".json", StringComparison.OrdinalIgnoreCase)
            ? path.Substring(0, path.Length - 5)
            : path;
    }

    private static string GetFullPath(string relativePath, PathLocation location)
    {
        if (!relativePath.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
        {
            relativePath += ".json";
        }

        return location switch
        {
            PathLocation.PersistentData => Path.Combine(Application.persistentDataPath, relativePath),
            PathLocation.StreamingAssets => Path.Combine(Application.streamingAssetsPath, relativePath),
            PathLocation.Resources => Path.Combine(Application.dataPath, "Resources", relativePath),
            _ => relativePath
        };
    }

    #endregion
}