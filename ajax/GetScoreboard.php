<?php
	require './internal/dbConnect.php';
	$sqlUserID = "Select * FROM users WHERE UserName = ?";
	$sqlUserInScore = "Select users.UserName, Scores.Score From users, Scores WHERE users.UserID = Scores.UserID AND users.UserID = ?";
	$sqlScoreBoard = "SELECT * FROM TopTenView UNION $sqlUserInScore";
	
	$username = $_POST['username'];
	$pin = $_POST['pin'];
	
	if(!($stmtScoreboard = $mysqli->prepare($sqlScoreBoard))){
		echo "SQL Error(" . $mysqli->errno . "): " . $mysqli->error;
		exit;
	}
		
	$stmtScoreboard->bind_param("i", $_SESSION['UserID']);
		
	if(!$stmtScoreboard->execute()){
		echo "SQL Error(" . $mysqli->errno . "): " . $mysqli->error;
		exit;
	}
		
	$scoreboardResult = $stmtScoreboard->get_result();
	$Scoreboard = array();
	while($row = $scoreboardResult->fetch_array()){
		$Scoreboard[$row['UserName']] = $row['Score'];
	}
	
	
	echo json_encode($Scoreboard);
	
	$mysqli->close();
?>