// See https://aka.ms/new-console-template for more information
using AutoHardLink;

var source = "D:\\Cache-Siemens\\nvd-links-crawler-data\\";
var target = "F:\\";

var linkFileNamesByHash = new Dictionary<string, string>();

var hardlinks = 0;
var allItems = Directory.EnumerateFiles(source, "*", SearchOption.AllDirectories).ToArray();
for (int it = 0; it < allItems.Length; it++)
{
    if ((it % 1000) == 0)
    {
        Console.WriteLine($"{it} of {allItems.Length} ({(float)it / allItems.Length * 100:0.0})%");
        Console.WriteLine($"    Hard links = {hardlinks} ({(float)hardlinks / it * 100:0.0}%)");
    }
    string? sourceFileFullName = allItems[it];
    var hash = Helpers.SHA256CheckSum(sourceFileFullName);
    if (!linkFileNamesByHash.TryGetValue(hash, out var linkFileRelativeName))
    {
        linkFileRelativeName = sourceFileFullName.Replace(source, "");
        linkFileNamesByHash[hash] = linkFileRelativeName;
    }
    var fileRelativeName = sourceFileFullName.Replace(source, "");
    var targetFileFullName = target + fileRelativeName;
    var linkFileFullName = target + linkFileRelativeName;
    if (!File.Exists(targetFileFullName))
    {
        Directory.CreateDirectory(Directory.GetParent(targetFileFullName).FullName);
        if (targetFileFullName == linkFileFullName)
        {
            File.Copy(sourceFileFullName, targetFileFullName);
        }
        else
        {
            var result = Helpers.CreateHardLink(targetFileFullName, linkFileFullName, 0);
            if (result)
            {
                hardlinks++;
            }
            else
            {
                File.Copy(sourceFileFullName, targetFileFullName);
                linkFileRelativeName = sourceFileFullName.Replace(source, "");
                linkFileNamesByHash[hash] = linkFileRelativeName;
            }
        }
    }
}