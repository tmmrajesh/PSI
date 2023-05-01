program App;
var 
   i, j : integer;

procedure foo () ;
var 
   i : string;
begin
   j := 5;
   i := j;
end;

begin
   foo ();
   writeln (j);
end.