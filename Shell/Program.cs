using PSI;

static class Start {
   static void Main () {
      Test ("../Shell/Demo/Tests/Z/Complex.pas");
      // Test ("../Shell/Demo/Tests/B/Init07.pas");

      //// All Tests
      //Test (Directory.GetFiles ("../Shell/Demo/Tests", "*.pas", SearchOption.AllDirectories));

      //// Initializations only
      //Test (Directory.GetFiles ("../Shell/Demo/Tests/D", "*.pas", SearchOption.AllDirectories));
   }

   static void Test (params string [] files) {
      NProgram? node;
      foreach (var file in files) {
         Console.WriteLine ($"\n\n[{Path.GetFileName (file)}]\n");
         try {
            var text = File.ReadAllText (file);
            node = new Parser (new Tokenizer (text)).Parse ();
            node.Accept (new TypeAnalyze ());
            node.Accept (new PSIPrint ());
         } catch (ParseException pe) {
            pe.Print ();
         } catch (Exception e) {
            Console.WriteLine ();
            Console.WriteLine (e);
         }
      }
   }
}