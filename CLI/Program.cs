using System;
using System.IO;

namespace CLI
{
    class Program
    {

        static void Main(string[] args)
        {
            Console.WriteLine("Provide a repo you would like to explore(Default repos do not work with this)");
            string repo = Console.ReadLine();
            if (!repo.EndsWith('/')) repo += '/';

            // TODO
            // do .repo files that contains the repo data, add refresh button to recreate .repo file
            Repo r = new Repo(repo);

            for (; ; ) {
                Console.WriteLine("Type a command or package (current repo: {0})", r.name);
                Console.Write("$$$ ");
                string search = Console.ReadLine();
                if (search == "" || search == "exit") break;
                if (search == "cls") {
                    Console.Clear();
                    continue;
                }
                if (search == "list") {
                    Console.WriteLine("Package that exists on {0}:", r.name);
                    foreach(Package pakk in r.packages.Values) {
                        Console.WriteLine(pakk.package);
                    }
                    continue;
                }
                if (search == "help") {
                    // TODO
                    continue;
                }
                if (r.packages.TryGetValue(search, out Package pak)) {
                    Console.WriteLine(" Name: " + pak.name);
                    Console.WriteLine(" Description: " + pak.description);
                    Console.WriteLine(" Section: " + pak.section);
                    Console.WriteLine(" MD5: " + pak.md5);
                    Console.WriteLine(" Size: " + pak.size);
                    Console.WriteLine(" Link: " + pak.url);
                } else {
                    Console.WriteLine("Package not found");
                }
            }
            Directory.Delete(r.name, true);
            File.Delete("Release");
        }
    }
}