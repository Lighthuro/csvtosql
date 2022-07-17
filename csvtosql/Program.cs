// See https://aka.ms/new-console-template for more information
string csvFileFullName;
char separator;

separator = ',';//comma
csvFileFullName= $"{Environment.CurrentDirectory}\\file.csv";
if (Environment.GetCommandLineArgs().Length>1 && !String.IsNullOrEmpty(Environment.GetCommandLineArgs()[1]))
{
    csvFileFullName = Environment.GetCommandLineArgs()[1];
    if (Environment.GetCommandLineArgs().Length > 2 && !String.IsNullOrEmpty(Environment.GetCommandLineArgs()[2]))
    {
        separator = Environment.GetCommandLineArgs()[2][0];
    }
}


string[] lines=File.ReadAllLines(csvFileFullName);
string[] columns=lines[0].Split(separator);
string columnsQuery()
{
    string snippet="";
    foreach(string column in columns)
    {
        snippet += $"\t{column} VARCHAR(70),\n";
    }
    return snippet.Remove(snippet.Length-2).Replace('"','\'');
}
string InsertDataQuery()
{
    string snippet = "";
    string valuesWritter(string[] values)
    {
        string formatedString = "";
        foreach (string value in values)
        {
            formatedString += $"{value},";
        }
        return formatedString.Remove(formatedString.Length-1);
    }
    for (int i = 1; i < lines.Length; i++)
    {
        string line = lines[i];
        string[] datas = line.Split(separator);
        snippet += $"\t({valuesWritter(datas)}),\n";        
    }
    return snippet.Remove(snippet.Length - 2);
    
}
string tableName = Path.GetFileNameWithoutExtension(csvFileFullName);
string tableCreationQuery = @$"
CREATE TABLE {tableName}
(
{columnsQuery()}
);";
Console.WriteLine(tableCreationQuery);

string tableInsertionQuery = $@"
INSERT INTO {tableName} values
{InsertDataQuery()};
";
Console.WriteLine(tableInsertionQuery);
string outputFileName = $"{Environment.CurrentDirectory}\\Output\\{tableName}.sql";
string outputDirectory=Path.GetDirectoryName(outputFileName);
if (Directory.Exists(outputDirectory))
{
    Directory.Delete(outputDirectory,true);
}
Directory.CreateDirectory(outputDirectory);
File.WriteAllText(outputFileName, $"{tableCreationQuery}\n{tableInsertionQuery}");
