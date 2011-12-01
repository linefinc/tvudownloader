<?php
//XML output of an existing MySql database
header("Content-type: text/xml");

echo "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
echo "<versions>";
echo "<last>0.5.2</last>";
echo '<version vernumber = "0.5.1"><changelog></changelog></version>';
echo '<version vernumber = "0.4.1"><changelog></changelog></version>';
echo '<version vernumber = "0.4.0"><changelog></changelog></version>';
echo '</versions>';



	$ip = getenv('REMOTE_ADDR');
	$fp = fopen('./data/data.csv','a');
	
	if($fp)
	{
		//fseek($fp,0,SEEK_END);
		$tvuid = $_GET['tvuid'];
		$str = date('Y-m-d H:i:s.0T') . ";" .$ip . ";" . $tvuid . "\n";

		fwrite($fp, $str);
		fclose($fp);
	}

?> 