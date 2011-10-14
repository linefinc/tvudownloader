<?php
//XML output of an existing MySql database
header("Content-type: text/xml");

echo "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
echo "<versions>";
echo "<last>0.5.1</last>";
echo '<version vernumber = "0.4.1"><changelog></changelog></version>';
echo '<version vernumber = "0.4.0"><changelog></changelog></version>';
echo '</versions>';
?> 