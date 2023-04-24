// ⓅⓈⒾ  ●  Pascal Language System  ●  Academy'23
// PSIPrint.cs ~ Prints a PSI syntax tree in Pascal format
// ─────────────────────────────────────────────────────────────────────────────
namespace PSI;

public class PSIPrint : Visitor<StringBuilder> {
   public override StringBuilder Visit (NProgram p) {
      Write ($"program {p.Name}; ");
      Visit (p.Block);
      return Write (".");
   }

   public override StringBuilder Visit (NBlock b) 
      => Visit (b.Decls, b.Body);

   public override StringBuilder Visit (NDeclarations d) {
      if (d.Vars.Length > 0) {
         NWrite ("var"); N++;
         foreach (var g in d.Vars.GroupBy (a => a.Type))
            NWrite ($"{g.Select (a => a.Name).ToCSV ()} : {g.Key};");
         N--;
      }

      if (d.Methods.Length > 0) {
         NWrite ("");
         N++;
         foreach (var method in d.Methods) {
            Visit (method);
            Write (";\n");
         }
         N--;
      }

      return S;
   }

   public override StringBuilder Visit (NVarDecl d)
      => NWrite ($"{d.Name} : {d.Type}");

   public override StringBuilder Visit (NFnDecl d) {
      NWrite ($"function {Declaration (d)} : {d.Type};");
      return Visit (d.Block);
   }

   public override StringBuilder Visit (NProcDecl d) {
      NWrite ($"procedure {Declaration (d)};"); 
      return Visit (d.Block);
   }

   string Declaration (NMethodDecl d) {
      return $"{d.Name.Text} ({ParamsList (d.Params.GroupBy (x => x.Type))})";

      string ParamsList (IEnumerable<IGrouping<NType, NVarDecl>> VarsList) => VarsList.Select (Params).ToCSV ("; ");
      string Params (IGrouping<NType, NVarDecl> Vars) => $"{Vars.Select (x => x.Name.Text).ToCSV ()}: {Vars.Key}";
   }

   public override StringBuilder Visit (NCompoundStmt b) {
      NWrite ("begin"); N++;  Visit (b.Stmts); N--; return NWrite ("end"); 
   }

   public override StringBuilder Visit (NAssignStmt a) {
      NWrite ($"{a.Name} := "); a.Expr.Accept (this); return Write (";");
   }

   public override StringBuilder Visit (NCallStmt c) {
      NWrite ($"{c.Name} (");
      for (int i = 0; i < c.Params.Length; i++) {
         if (i > 0) Write (", "); c.Params[i].Accept (this);
      }
      return Write (");");
   }

   public override StringBuilder Visit (NWriteStmt w) {
      NWrite (w.NewLine ? "WriteLn (" : "Write (");
      for (int i = 0; i < w.Exprs.Length; i++) {
         if (i > 0) Write (", ");
         w.Exprs[i].Accept (this);
      }
      return Write (");");
   }

   public override StringBuilder Visit (NReadStmt s) 
      => NWrite ($"Read ({s.Identifiers.Select (x => x.Text).ToCSV ()});");

   public override StringBuilder Visit (NForStmt s) {
      NWrite ($"for {s.LoopVar} := "); Visit (s.InitialExpr);
      Write (s.Inc ? " to " : " downto "); Visit (s.FinalExpr); Write (" do");
      N++; Visit (s.Stmt); N--;
      return S;
   }

   public override StringBuilder Visit (NLiteral t)
      => Write (t.Value.ToString ());

   public override StringBuilder Visit (NIdentifier d)
      => Write (d.Name.Text);

   public override StringBuilder Visit (NUnary u) {
      Write (u.Op.Text); return u.Expr.Accept (this);
   }

   public override StringBuilder Visit (NBinary b) {
      Write ("("); b.Left.Accept (this); Write ($" {b.Op.Text} ");
      b.Right.Accept (this); return Write (")");
   }

   public override StringBuilder Visit (NFnCall f) {
      Write ($"{f.Name} (");
      for (int i = 0; i < f.Params.Length; i++) {
         if (i > 0) Write (", "); f.Params[i].Accept (this);
      }
      return Write (")");
   }

   public override StringBuilder Visit (NIfStmt s) {
      NWrite ($"if "); s.Expr.Accept (this); Write (" then ");
      N++; s.ThenStmt.Accept (this); N--;
      if (s.ElseStmt != null) {
         NWrite ("else");
         N++;  s.ElseStmt.Accept (this); N--;
      }
      return S;
   }

   public override StringBuilder Visit (NRepeatStmt s) {
      NWrite ($"repeat "); 
      N++; Visit (s.Stmts); N--;
      NWrite ("until " ); s.Expr.Accept (this); Write (";");
      return S;
   }

   public override StringBuilder Visit (NWhileStmt s) {
      NWrite ($"while "); Visit (s.Expr); Write (" do ");
      N++; s.Stmt.Accept (this); N--;
      return Write (";");
   }

   StringBuilder Visit (params Node[] nodes) {
      nodes.ForEach (a => a.Accept (this));
      return S;
   }

   // Writes in a new line
   StringBuilder NWrite (string txt) 
      => Write ($"\n{new string (' ', N * 3)}{txt}");
   int N;   // Indent level

   // Continue writing on the same line
   StringBuilder Write (string txt) {
      Console.Write (txt);
      S.Append (txt);
      return S;
   }

   readonly StringBuilder S = new ();
}