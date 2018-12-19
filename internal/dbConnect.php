<?php
	$user = "CrazyRoadWebUser";
	$password = "CrazyRoadClient";
	$host = "";
	$db = "CrazyRoadDB";
	
	//Connect to database with Unix Socket
	$mysqli = new mysqli($host, $user, $password, $db, null,"/home/student/it/2014/it144287/mysql/run/mysql.sock");
	if(!$mysqli){
		echo "Failed to connect to MySQL: (" .
		$mysqli->connect_errno . ") " . $mysqli->connect_error;
	}
?>