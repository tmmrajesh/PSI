program App;
var 
   i : integer;
   j, k: real;

begin
   read (i, j, k);

   if i < 12 then 
      i := 12;
   else 
      j := 13;

   while j < 20 do begin
      k := k + j;
      j := j - 1;
   end;

   for i := 1 to 20 do 
      j := j + i;

   repeat 
      writeln ("Hello");
      j := j - 1;
   until j <= 0;

end.