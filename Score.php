<?php
	Session_start();
	if($_POST['requestType'] === "submit"){
		require "./ajax/SubmitScore.php";
	}
	else{
		require "./ajax/GetScoreboard.php";
	}
?>