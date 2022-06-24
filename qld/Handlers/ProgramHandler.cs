using gpm;
using gpm.Common;
using Newtonsoft.Json;
using qld.DataTemplates;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static qld.Program;

namespace qld.Handlers
{
    public static class ProgramHandler
    {
        static string cacheFolder= Path.Combine( System.IO.Path.GetTempPath(),"qld");
        static string metadataFolder = Path.Combine(System.IO.Path.GetTempPath(), "qld","metadata");
        const string REPO = "http://qlcloud.sylu.edu.cn/api/list/1?path=software/";


        private static List<MetaData.FilesItem> MatchProgram(string target, List<MetaData.FilesItem> files)
        {
            var r = new List<MetaData.FilesItem>();

            foreach (var item in files)
            {
                if (target.IndexOf("@")==-1)
                {
                    var fi = item.name.Split("@");
                    if (fi.Count()==2)
                    {
                        if (fi[0].StartsWith(target))
                        {
                            r.Add(item);
                        }
                    }
                }
                else if (target==item.name)
                {
                    r.Append(item);

                }
            }

            return r;
        }




        public static async Task Update()
        {

            Request request = new Request();

            await AnsiConsole.Status()
                .Start("Fetching...",async ctx =>
                {
                    // Simulate some work
                    AnsiConsole.MarkupLine($"Fetching metadata from {REPO}...");


                    var r = await request.Get(REPO);
                    // Update the status and spinner


                    if (!Directory.Exists(metadataFolder))
                    {
                        Directory.CreateDirectory(metadataFolder);
                    }

                    File.WriteAllText(Path.Combine(metadataFolder, "metadata.json"), r);

                    AnsiConsole.MarkupLine($"Updated metadata successfully.");

                });


        }


        public static async Task Add(List<string> pkgs)
        {
            var index = 0;
            if (File.Exists(Path.Combine(metadataFolder, "metadata.json")))
            {
                var r = File.ReadAllText(Path.Combine(metadataFolder, "metadata.json"));
                var metadata = JsonConvert.DeserializeObject<MetaData.Root>(r);
                var files = metadata.data.files;
                foreach (var item in pkgs)
                {
                    //MetaData.FilesItem temp = files.Find(t => t.name == item);
                    List< MetaData.FilesItem> temps = MatchProgram(item,files);


                    if (temps.Count == 0)
                    {
                        MsgHelper.E($"Can't found a plugin named {item}");
                        continue;
                    }
                    MetaData.FilesItem temp;

                    if (temps.Count==1)
                    {
                        temp = temps[0];
                    }
                    else
                    {

                        string[] selections = (from d in temps select d.name).ToArray();
                        
                        var fruit = AnsiConsole.Prompt(
                            new SelectionPrompt<string>()
                                .Title("Which [green]version[/] you want to install?")
                                .PageSize(10)
                                .MoreChoicesText("[grey](Move up and down to reveal more files)[/]")
                                .AddChoices(selections));

                        temp = temps.Find(t => t.name == fruit);


                        // Echo the fruit back to the terminal
                        //AnsiConsole.WriteLine($"I agree. {fruit} is tasty!");
                    }

                    var downLoadUrl = temp.url;
                    var filep = Path.GetFileName(downLoadUrl);

                    MsgHelper.I(Markup.Escape($"[{index}/{pkgs.Count}] Installing {temp.name}"));


                    //MsgHelper.I($"Updatelog \n{new Panel( Markup.Escape( pluginInfo.body))}");

                    //var rule = new Rule("[green]更新日志[/]");

                    //AnsiConsole.Write(rule);

                    //AnsiConsole.WriteLine(Markup.Escape(pluginInfo.body));

                    var localPath = "";

                    // Asynchronous
                    await AnsiConsole.Progress()
                        .StartAsync(async ctx =>
                        {
                        // Define tasks
                        var task1 = ctx.AddTask("[green]DownLoading[/]");

                            localPath = Path.Combine(cacheFolder, filep);
                            await FileDownLoader.DownloadFileData(downLoadUrl, localPath, delegate (int a) {
                                task1.Increment(a - task1.Percentage);
                            });

                        });


                    MsgHelper.I($"Successfully downloaded {item}");
                    MsgHelper.I($"Preparing install {item}");


                    Process p = new Process();
                    // 设定程序名
                    p.StartInfo.FileName = localPath;
                    try
                    {
                        p.Start();

                    }
                    catch
                    {

                    }

                    p.WaitForExit();

                    index += 1;
                }

            }
            else
            {
                MsgHelper.W("Can't find metadata,Please run [bold]qld update[/] first.");

            }

        }

        public static async Task Remove(List<string> pkg)
        {
            MsgHelper.E("功能未完善！");
        }

        public static async Task List()
        {
            MsgHelper.E("功能未完善！");
        }
        public static async Task ListRepo()
        {
            if (File.Exists(Path.Combine(metadataFolder, "metadata.json")))
            {
                MsgHelper.W("Reading cached metadata...");

                var r = File.ReadAllText(Path.Combine(metadataFolder, "metadata.json"));
                var metadata = JsonConvert.DeserializeObject<MetaData.Root>(r);
                var files = metadata.data.files;


                // Create a table
                var table = new Table();

                // Add some columns
                table.AddColumn(new TableColumn("name").Centered());
                table.AddColumn(new TableColumn("time").Centered());
                table.AddColumn(new TableColumn("size").Centered());
                table.AddColumn(new TableColumn("path").Centered());


                foreach (var item in files)
                {
                    table.AddRow(item.name,item.time , item.size.ToString(),item.path);
                    //, , 
                }
                // Render the table to the console
                AnsiConsole.Write(table);

            }
            else
            {
                MsgHelper.W("Can't find metadata,Please run [bold]qld update[/] first.");

            }
        }


    }
}
