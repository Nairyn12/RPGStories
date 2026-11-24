#if UNITY_EDITOR
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

public static class ProjectDigestExporter
{
    // Поправь под свою структуру
    private static readonly string[] Roots = { "Assets/Scripts", "Assets/ScriptableObjects" };

    [MenuItem("Tools/Digest/Export API only")]
    private static void ExportApi() => Export(false);

    [MenuItem("Tools/Digest/Export Full code")]
    private static void ExportFull() => Export(true);

    private static void Export(bool full)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"# Project Digest — {Application.productName}");
        sb.AppendLine($"Unity {Application.unityVersion} | режим: {(full ? "FULL" : "API")}");

        // Сцены в билде
        sb.AppendLine("\n## Scenes");
        foreach (var s in EditorBuildSettings.scenes)
            sb.AppendLine($"- {(s.enabled ? "+" : "-")} {s.path}");

        // Пакеты
        var manifest = Path.Combine(Directory.GetParent(Application.dataPath).FullName, "Packages/manifest.json");
        if (File.Exists(manifest))
            sb.AppendLine("\n## Packages\n```json\n" + File.ReadAllText(manifest) + "\n```");

        var files = Roots.Where(Directory.Exists)
            .SelectMany(r => Directory.GetFiles(r, "*.cs", SearchOption.AllDirectories))
            .OrderBy(f => f).ToArray();

        sb.AppendLine("\n## File tree");
        foreach (var f in files)
            sb.AppendLine($"- {f} ({File.ReadAllLines(f).Length} строк)");

        int total = 0;
        foreach (var f in files)
        {
            var lines = File.ReadAllLines(f);
            total += lines.Length;
            sb.AppendLine($"\n## {f}\n```csharp");

            bool prevWasAttribute = false;
            foreach (var raw in lines)
            {
                string t = raw.Trim();
                bool keep = full
                    || t.StartsWith("[")
                    || prevWasAttribute                                  // [SerializeField] private ...
                    || t.StartsWith("public") || t.StartsWith("protected")
                    || t.Contains("class ") || t.Contains("interface ")
                    || t.Contains("struct ") || t.Contains("enum ")
                    || t.Contains("event ") || t.Contains("delegate ")
                    || t.StartsWith("namespace");

                if (keep) sb.AppendLine(raw);
                prevWasAttribute = t.StartsWith("[");
            }
            sb.AppendLine("```");
        }

        sb.AppendLine($"\n<!-- Всего строк: {total}, символов: {sb.Length} (~{sb.Length / 4} токенов) -->");

        var outPath = Path.Combine(Directory.GetParent(Application.dataPath).FullName,
            full ? "Digest_Full.md" : "Digest_API.md");
        File.WriteAllText(outPath, sb.ToString());
        Debug.Log($"Готово: {outPath} (~{sb.Length / 4} токенов)");
        EditorUtility.RevealInFinder(outPath);
    }
}
#endif