using NLog;
using System.ComponentModel.DataAnnotations;
using Helper;

string path = Directory.GetCurrentDirectory() + "/nlog.config";
// create instance of Logger
var logger = LogManager.LoadConfiguration(path).GetCurrentClassLogger();

string linksPath = "./library/links.csv";
string moviesPath = "./library/movies.csv";
string ratingsPath = "./library/ratings.csv";
string tagsPath = "./library/tags.csv";

string? resp;
do  {
Console.WriteLine("\nEnter 1 to add movie to library.");
Console.WriteLine("Enter 2 to view movie library.");
Console.WriteLine("Enter anything else to quit.");

resp = Console.ReadLine();

    //Adding a movie to the library
    if (resp == "1")    {
        Console.WriteLine("Add movie to library");

        //Checking to see if all files are in the library folder
        if (dataPathExists()) {

            AppendMovie();

        }   else    {
            logger.Fatal("Not all files are present in the library folder. Ensure links.csv, movies.csv, ratings.csv, and tags.csv are in the library folder.");
            resp = "3";
        }

    //Parsing the files in the library folder
    }   else if (resp == "2")   {
        if (dataPathExists()) {
            int i = 1;
            StreamReader sr = new StreamReader(moviesPath);
            while (!sr.EndOfStream) {

                //Fetches and separtes movie titles from the ID
                //ID: arrLine[0]
                //Title: movieTitle
                //Genres: movieGenres
                string line = sr.ReadLine();
                string[] arrLine = line.Split(',');
                string[] arrTitle = arrLine.SkipLast(1).ToArray();
                arrTitle = arrTitle.Skip(1).ToArray();
                string movieTitle = String.Join(",", arrTitle);
                movieTitle = movieTitle.Replace("\"", "");

                //Splits the genres from the titles, then joins them
                string genreSeparator = line.Split(',').Last();
                string[] arrGenre = genreSeparator.Split('|');
                string movieGenres = String.Join(", ", arrGenre);

                Console.WriteLine("\nID:{0} - {1}\nGenres: {2}", arrLine[0], movieTitle, movieGenres);
                i += 1;
            } sr.Close();

        }   else    {
            logger.Fatal("Not all files are present in the library folder. Ensure links.csv, movies.csv, ratings.csv, and tags.csv are in the library folder.");
            resp = "3";
        }


        }   else    {
        Console.WriteLine("Thank you for using KennethSoft 'Movie Library' software!");
    }
}   while (resp == "1" || resp == "2");



//Checks the library folder for the correct files
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



void AppendMovie() {

    // Assigns each ID into an array and checks if the ID is present in the database.
    int[] idArray = new int[]{};
    StreamReader sr = new StreamReader(moviesPath);
    while (!sr.EndOfStream) {
            //Fetches the ID
            string line = sr.ReadLine();
            string[] arrLine = line.Split(',');
            int idInt;
            bool isSuccess = Int32.TryParse(arrLine[0], out idInt);
            if (isSuccess) {
                idArray = idArray.Append(idInt).ToArray();
            }
        }
    sr.Close();

    StreamWriter sw = new StreamWriter(moviesPath, true);

    // Checks if the entered ID is an int and is already in the database.
    bool uniqueID = false;
    int movieID = 0;
    while (!uniqueID) {
        int checkID = Inputs.GetInt("Enter movie ID > ");
        if (idArray.Contains(checkID)){
            logger.Error("Error: ID is already present in the data. Please enter a unique ID or check if the movie is already present.");
        } else {
            movieID = checkID;
            uniqueID = true;
        }
    }

    //The title of the movie. Expects a non-null string
    string movieTitle = Inputs.GetString("Enter the title of the movie > ");

    //The year the movie was released. Expects a non-null integer
    int movieYear = Inputs.GetInt("Enter the year the movie was released > ");

    //Joins together the list of genres the user inputs
    bool isDone = false;
    string? genre;
    string[]? genreArr = new string[]{"(no genres listed)"};
    int i = 0;
    while (!isDone) {

        //Removes the (no genres listed) entry from the array after the first append
        if (i == 1) {
            genreArr = genreArr.Skip(1).ToArray();
        }

        //Allows the user to enter as many genres as they want until they enter a null value
        Console.WriteLine("Enter a genre (type nothing when done)");
        genre = Console.ReadLine();

        if (String.IsNullOrEmpty(genre)) {
            isDone = true;
        } else if (!String.IsNullOrEmpty(genre)){
            genreArr = genreArr.Append(genre).ToArray();
            i += 1;
        }
    }
    string movieGenres = String.Join("|", genreArr);

    Console.WriteLine("{0} was successfully added to the database.", movieTitle);
    sw.WriteLine("{0},{1} ({2}),{3}", movieID, movieTitle, movieYear, movieGenres);
    sw.Close();
}