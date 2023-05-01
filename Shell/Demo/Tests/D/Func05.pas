program App;

function Foo (i : integer) : integer;
var 
   i : integer;
   Foo : integer;
begin
   i := 3;
   Foo := i;
end;

begin
   writeln (Foo (1))
end.
