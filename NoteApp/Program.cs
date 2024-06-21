using System.Diagnostics;

string[] menuItems = [
    "View Topics",
    "Add Topic",
    "Quick Note",
    "View Notes",
    "Edit Notes",
    "Quit"
    ];
bool isDone = false;
string notesPath = "C:\\Users\\austi\\OneDrive\\Notes";
string[] topics;

while (!isDone)
{
    Console.Clear();
    topics = GetTopics();
    WriteMenu();
    Console.WriteLine("What would you like to do?");
    string? userInput = Console.ReadLine();
    if (userInput != null)
    {
        userInput = TrimAndLower(userInput);
    }
    switch (userInput)
    {
        case "view topics":
            ViewTopics();
            break;
        case "add topic":
            AddTopic();
            break;
        case "quick note":
            AddNote();
            break;
        case "view notes":
            ViewNotes();
            break;
        case "edit notes":
            EditNotes();
            break;
        case "quit":
            isDone = true;
            break;
        default:
            Console.WriteLine("Unrecognized input.");
            break;
    }
    Console.WriteLine("Hit any key to continue.");
    Console.ReadKey();
}

void WriteMenu()
{
    foreach (string item in menuItems)
    {
        Console.Write(item + " || ");
    }
    Console.Write("\n");
}

void AddTopic()
{
    Console.WriteLine("What topic do you want to add?");
    string? newTopic = Console.ReadLine();

    if (newTopic == null || newTopic == "") { return; }

    newTopic = TrimAndLower(newTopic);
    if (topics.Contains(newTopic))
    {
        Console.WriteLine(newTopic + " is already a topic. Here are the topics already used:");
        ViewTopics();
    }
    else
    {
        File.Create(notesPath + "\\" + newTopic + ".txt").Dispose();
    }
}

void ViewTopics()
{
    foreach (string topic in topics) { Console.WriteLine("\t" + topic); }
}

void AddNote()
{
    Console.WriteLine("Which topic do you want to add a note for? Here are the available topics:");
    string? topicChoice = GetTopicFromUser();
    if (topicChoice == null) { return; }

    Console.WriteLine("Add your note:");
    using (StreamWriter outputFile = new StreamWriter(Path.Combine(notesPath, topicChoice + ".txt"), true))
    {
        string? newNote = Console.ReadLine();
        if (newNote != null) { outputFile.WriteLine(newNote); }
    }
}

string? GetTopicFromUser()
{
    ViewTopics();
    string? topicChoice = Console.ReadLine();
    if (topicChoice == null) { return topicChoice; }
    topicChoice = TrimAndLower(topicChoice);
    if (!topics.Contains(topicChoice))
    {
        Console.WriteLine("Topic not found.");
        return null;
    }
    return topicChoice;
}

string[] GetTopics()
{
    string[] results = Directory.GetFiles(notesPath);
    for (int i = 0; i < results.Length; i++)
    {
        results[i] = results[i].Replace(notesPath + "\\", "");
        results[i] = results[i].Replace(".txt", "");
    }
    return results;
}

void ViewNotes()
{
    Console.WriteLine("Which topic would you like to view the notes for?");
    string? topicChoice = GetTopicFromUser();
    if (topicChoice == null) { return; }
    try
    {
        using StreamReader reader = new(notesPath + "\\" + topicChoice + ".txt");
        string text = reader.ReadToEnd();
        Console.WriteLine("\n" + text);
    }
    catch (IOException e)
    {
        Console.WriteLine("The file could not be read.");
        Console.WriteLine(e.Message);
    }
}

void EditNotes()
{
    Console.WriteLine("Which topic would you like to edit the notes for?");
    string? topicChoice = GetTopicFromUser();
    if (topicChoice == null) { return; }
    Process.Start("notepad.exe", notesPath + "\\" + topicChoice + ".txt");
}

string TrimAndLower(string inString)
{
    string result = inString.Trim();
    return result.ToLower();
}
