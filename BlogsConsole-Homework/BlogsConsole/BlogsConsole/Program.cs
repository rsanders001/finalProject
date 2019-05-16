using NLog;
using BlogsConsole.Models;
using System;
using System.Linq;
using System.Collections.Generic;

namespace BlogsConsole
{
    class MainClass
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public static void Main(string[] args)
        {


            Boolean blogIsRunning = true;
            logger.Info("Program started");
            try
            {

                while (blogIsRunning == true)
                {



                    Console.WriteLine("Select An Option");
                    Console.WriteLine("1.)Add A Blog");
                    Console.WriteLine("2.)Display All Blogs");
                    Console.WriteLine("3.)Create Post");
                    Console.WriteLine("4.)Display Posts");
                    int response = Convert.ToInt32(Console.ReadLine());



                    if (response == 1)
                    {
                        logger.Info("Option 1 Selected");
                        // Create and save a new Blog
                        
                        var nameLoop = true;
                        var name = "";
                        while (nameLoop == true) {
                            Console.Write("Enter a name for a new Blog: ");
                            name = Console.ReadLine();
                            if (name.Trim() == "")
                            {
                                logger.Info("No Blog Name Found");
                                Console.WriteLine("Enter a valid Blog Name");
                            }
                            else
                            {
                                nameLoop = false;
                            }

                                                       
                        }
                        var blog = new Blog { Name = name };
                        var db = new BloggingContext();
                        db.AddBlog(blog);
                        logger.Info("Blog added - {name}", name);
                    }

                    else if (response == 2)
                    {
                        logger.Info("Option 2 Selected");
                        var db = new BloggingContext();
                        // Display all Blogs from the database
                        var query = db.Blogs.OrderBy(b => b.BlogId);
                        var counter = db.Blogs.Count();
                        if (counter > 0)
                        {
                            Console.WriteLine("All blogs in the database:");

                            Console.WriteLine(counter + " Blogs Found");
                            foreach (var item in query)
                            {
                                Console.WriteLine(item.Name);
                            }
                        }

                        else
                        {
                            Console.WriteLine("No Blogs Stored");
                            logger.Error("No Blogs Stored");
                        }


                    }
                    else if (response == 3)
                    {
                        logger.Info("Option 3 Selected");
                        var db = new BloggingContext();
                        var query = db.Blogs.OrderBy(b => b.BlogId);
                        Console.WriteLine("All blogs in the database:");

                        List<int> blogIDArray = new List<int> { };



                        foreach (var item in query)
                        {
                            Console.WriteLine("ID: " + item.BlogId.ToString() + " NAME: " + item.Name);
                            blogIDArray.Add(item.BlogId);
                        }
                        Boolean valid = true;

                        while (valid == true)
                        {
                            Console.WriteLine("Select Blog You Would Like To Change: ");
                            int userChange = Convert.ToInt32(Console.ReadLine());
                            int chosenBlogID = 0;

                            for (int i = 0; i < blogIDArray.Count; i++)
                            {
                                if (userChange == blogIDArray.ElementAt(i))
                                {
                                    valid = false;
                                    chosenBlogID = blogIDArray.IndexOf(blogIDArray.ElementAt(i));
                                }

                            }
                        
                            if (valid == true)
                            {
                                Console.WriteLine("PLEASE ENTER A VALID ID");
                            }
                            else
                            {


                                var validLoop = true;
                                var title = "";
                                while (validLoop == true)
                                {
                                    Console.WriteLine("Enter Post Title: ");
                                    title = Console.ReadLine();
                                    if(title.Trim() == "")
                                    {
                                        logger.Info("No Title Added For Post");
                                        Console.WriteLine("Please Add a Post Title");
                                    }
                                    else
                                    {
                                        validLoop = false;
                                    }

                                }

                                Console.WriteLine("Enter Post Details: ");
                                var details = Console.ReadLine();

                                var newID = 1;


                                try
                                {
                                    newID = db.Posts.Select(p => p.PostId).Last() + 1;
                                }
                                catch
                                {
                                    Console.WriteLine("Post Stored");
                                }


                                var post = new Post { Title = title, Content = details, BlogId = chosenBlogID + 1, PostId = newID };
                                db.AddPost(post);
                                logger.Info("Post added - {title}", post);


                                // Display all Blogs from the database
                                var query2 = db.Blogs.OrderBy(b => b.Name);
                                Console.WriteLine("Posts In Database:");
                                foreach (var item in query2)
                                {
                                    Console.WriteLine(item.Name);
                                }
                            }


                        }





                    }
                    else if(response == 4)
                    {

                        logger.Info("Option 4 Selected");
                        var validLoop = true;

                        while (validLoop == true)
                        {
                            var db = new BloggingContext();
                            var query = db.Blogs.OrderBy(b => b.BlogId);
                            Console.WriteLine("All blogs in the database:");

                            List<int> blogIDArray = new List<int> { };
                            Console.WriteLine("0.) Display All Posts");



                            foreach (var item in query)
                            {
                                Console.WriteLine("ID: " + item.BlogId.ToString() + " NAME: " + item.Name);
                                blogIDArray.Add(item.BlogId);
                            }


                            var userResponse = Convert.ToInt32(Console.ReadLine());
                            if (userResponse == 0)
                            {

                                
                                // Display all Blogs from the database
                                var query3 = db.Posts.OrderBy(b => b.PostId);
                                var counter = db.Posts.Count();
                                if (counter > 0)
                                {
                                    Console.WriteLine("All Posts in the database:");

                                    Console.WriteLine(counter + " Posts Found");
                                    foreach (var item in query3)
                                    {
                                        Console.WriteLine("Title: "+ item.Title + "\nContent: " + item.Content);
                                    }
                                }

                                else
                                {
                                    Console.WriteLine("No Posts Stored");
                                    logger.Error("No Posts Stored");
                                }
                                validLoop = false;

                            }
                            else
                            {
                                int chosenBlogID = 0;

                                for (int i = 0; i < blogIDArray.Count; i++)
                                {
                                    if (userResponse == blogIDArray.ElementAt(i))
                                    {
                                        validLoop = false;
                                        chosenBlogID = blogIDArray.IndexOf(blogIDArray.ElementAt(i));
                                    }

                                }

                                if (validLoop == true)
                                {
                                    Console.WriteLine("PLEASE ENTER A VALID ID");
                                }
                                else
                                {
                                    var postByBlog = db.Posts.OrderBy(b => b.Title).Where(b => b.BlogId == chosenBlogID);
                                    foreach (var item in postByBlog)
                                    {
                                        Console.WriteLine("Title: " + item.Title + "\nContent: " + item.Content);
                                    }
                                }

                            }





                            





                        }




                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }

            Console.WriteLine("Press enter to quit");
            string x = Console.ReadLine();

            logger.Info("Program ended");
        }
    }
}
