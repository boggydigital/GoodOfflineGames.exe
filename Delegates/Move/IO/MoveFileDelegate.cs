using System.IO;

using Attributes;

using Interfaces.Delegates.Move;
using Interfaces.Delegates.Confirm;
using Interfaces.Delegates.Create;



namespace Delegates.Move.IO
{
    public class MoveFileDelegate : IMoveDelegate<string>
    {
        public void Move(string fromUri, string toUri)
        {
            if (!File.Exists(fromUri)) return;

            var destinationDirectory = Path.GetDirectoryName(toUri);
            if (!string.IsNullOrEmpty(destinationDirectory) &&
                !Directory.Exists(destinationDirectory))
                Directory.CreateDirectory(destinationDirectory);

            if (File.Exists(toUri))
                File.Delete(toUri);

            File.Move(fromUri, toUri);
        }
    }
}