using Tweetinvi;
using Tweetinvi.Parameters;

string[] keys = File.ReadAllLines(@"/home/ubuntu/github/config/casa-automatica-frame.casa");
//Line 0 of file contains CONSUMER_KEY
//Line 1 of file contains CONSUMER_SECRET
//Line 2 of file contains ACCESS_TOKEN
//Line 3 of file contains ACCESS_TOKEN_SECRET
TwitterClient userClient = new(keys[0], keys[1], keys[2], keys[3]);
int frame = Convert.ToInt32(File.ReadAllLines(@"/home/ubuntu/github/casa-automatica-frame/frame.txt")[0]);
try
{
    string[] pastas = Directory.GetDirectories(@"/home/ubuntu/github/casa-automatica-frame/frames/");
    foreach (string pasta in pastas)
    {
        string[] frames = Directory.GetFiles(pasta);
        for (int i = 0; i < frames.Length; i++)
        {
            try
            {
                var tweetinviLogoBinary = File.ReadAllBytes(frames[i]);
                var uploadedImage = await userClient.Upload.UploadTweetImageAsync(tweetinviLogoBinary);
                var tweetWithImage = await userClient.Tweets.PublishTweetAsync(
                    new PublishTweetParameters($"Todos os ep de Em Busca da Casa Automatica por Frame \n\n" +
                    $"Frame: {frame}\nEp {pasta.Replace(@"/home/ubuntu/github/casa-automatica-frame/frames/", "")}")
                {
                    Medias = { uploadedImage }
                });
                Console.WriteLine("Em busca da casa automatica, frame: " + frame);
                frame++;
                Thread.Sleep(36000);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                i--;
            }
        }
    }
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}
File.WriteAllText("/home/ubuntu/github/casa-automatica-frame/frame.txt", frame.ToString());
Console.ReadKey();