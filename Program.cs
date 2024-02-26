using NLog;
string path = Directory.GetCurrentDirectory() + "\\nlog.config";
// create instance of Logger
var logger = LogManager.LoadConfiguration(path).GetCurrentClassLogger();

string linksPath = ".\\library\\links.csv";
string moviesPath = ".\\library\\movies.csv";
string ratingsPath = ".\\library\\ratings.csv";
string tagsPath = ".\\library\\tags.csv";

string? resp;
do  {
Console.WriteLine("Enter 1 to add movie to library.");
Console.WriteLine("Enter 2 to view movie library.");
Console.WriteLine("Enter anything else to quit.");

resp = Console.ReadLine();

    //Adding a movie to the library
    if (resp == "1")    {
        Console.WriteLine("Add movie to library");

        //Checking to see if all files are in the library folder
        if (File.Exists(linksPath) && File.Exists(moviesPath) && File.Exists(ratingsPath) && File.Exists(tagsPath)) {
            Console.WriteLine("Hooray!");

            //TODO: Make constructors to populate the files

        }   else    {
            logger.Fatal("Not all files are present in the library folder. Ensure links.csv, movies.csv, ratings.csv, and tags.csv are in the library folder.");
            resp = "3";
        }

    //Parsing the files in the library folder
    }   else if (resp == "2")   {
        if (File.Exists(linksPath) && File.Exists(moviesPath) && File.Exists(ratingsPath) && File.Exists(tagsPath)) {
            int count = 0;
            StreamReader sr = new StreamReader(moviesPath);
            while (!sr.EndOfStream) {
                string? line = sr.ReadLine();
                string[] arrLine = line.Split(',');
                Console.WriteLine(arrLine);
                count += 1;
            } sr.Close();

        }   else    {
            logger.Fatal("Not all files are present in the library folder. Ensure links.csv, movies.csv, ratings.csv, and tags.csv are in the library folder.");
            resp = "3";
        }


        }   else    {
        Console.WriteLine("Thank you for using KennethSoft 'Movie Library' software!");
    }
}   while (resp == "1" || resp == "2");


bool dataPathExists() {
    if (!File.Exists(linksPath))    {  
        return false;
    }   else if (!File.Exists(moviesPath))  {
        return false;
    }   else if (!File.Exists(ratingsPath))    {
        return false;
    }   else if (!File.Exists(tagsPath))    {
        return false;
    }   else  {
    return true;
    }
}