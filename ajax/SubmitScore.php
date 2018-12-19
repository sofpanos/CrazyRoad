<?php
	require './internal/dbConnect.php';
	$sqlUserID = "Select UserID, pin FROM users WHERE UserName = ?";
	$sqlUserInScore = "Select * From Scores WHERE UserID = ?";
	
	$sqlInsertUser = "INSERT INTO `users`( `UserName`, `pin`) VALUES (?,?)";
	$sqlInsertScore = "INSERT INTO `Scores`(`UserID`, `Score`) VALUES (?,?)";
	
	$sqlUpdateScore = "UPDATE `Scores` SET `Score`= ? WHERE UserID = ?";
	
	$username = $_POST['username'];
	$pin = $_POST['pin'];
	$score = $_POST['score'];
	
	
	//Check If Users exists
	if(!($sqlStmtUser = $mysqli->prepare($sqlUserID))){
		echo "SQL Error(" . $mysqli->errno . "): " . $mysqli->error;
		exit;
	}
	
	$sqlStmtUser->bind_param("s", $username);
	
	if(!$sqlStmtUser->execute()){
		echo "SQL Error(" . $sqlStmtUser->errno . "): " . $sqlStmtUser->error;
		exit;
	}
	
	$userResult = $sqlStmtUser->get_result();
	
	if($userRow = $userResult->fetch_array()){
		if($userRow['pin'] != $pin){
			echo "Authorization Failed";
			exit;
		}
		else{
			$_SESSION['username'] = $username;
			$_SESSION['userID'] = $userRow['UserID'];
		}
		
		//Check if allready score submitted.
		if(!($stmtUserScore = $mysqli->prepare($sqlUserInScore))){
			echo "SQL Error(" . $mysqli->errno . "): " . $mysqli->error;
			exit;
		}
		
		$stmtUserScore->bind_param("i", $userRow['UserID']);
		
		if(!$stmtUserScore->execute()){
			echo "SQL Error(" . $stmtUserScore->errno . "): " . $stmtUserScore->error;
			exit;
		}
		
		$ScoreResult = $stmtUserScore->get_result();
		if($scoreRow = $ScoreResult->fetch_array()){
			//update user's score
			if(!($stmtUpdateUserScore = $mysqli->prepare($sqlUpdateScore))){
				echo "SQL Error(" . $mysqli->errno . "): " . $mysqli->error;
				exit;
			}
			$value = max($score, $scoreRow['Score']);
			$stmtUpdateUserScore->bind_param("ii", $value, $userRow['UserID']);
			
			if(!$stmtUpdateUserScore->execute()){
				echo "SQL Error(" . $msqli->errno . "): " . $mysqli->error;
				exit;
			}
			
			if($stmtUpdateUserScore->affected_rows > 0){
				echo "Score Submitted";
				exit;
			}
			else{
				echo "Your Highscore is $value, try again you can do it!";
				exit;
			}
		}
		else{
			//Shouldn't be here, because a user should only exist if he had submitted a score before
			echo "Unknown Error - Sorry for that try again or try another name";
			exit;
		}
	}
	else{
		//register user
		if(!($stmtInsert = $mysqli->prepare($sqlInsertUser))){
			echo "SQL Error(" . $mysqli->errno . "): " . $mysqli->error;
			exit;
		}
		
		$stmtInsert->bind_param("si", $username, $pin);
		
		if(!$stmtInsert->execute()){
			echo "SQL Error(" . $stmtInsert->errno . ")86: " . $stmtInsert->error;
			exit;
		}
		$userId = $stmtInsert->insert_id;
		$_SESSION['username'] = $username;
		$_SESSION['userID'] = $userId;
		//submit score
		
		if(!($stmtInsert = $mysqli->prepare($sqlInsertScore))){
			echo "SQL Error(" . $mysqli->errno . "): " . $mysqli->errror;
			exit;
		}
		
		$stmtInsert->bind_param("ii", $userId, $score);
		
		if(!$stmtInsert->execute()){
			echo "SQL Error(" . $stmtInsert->errno . "): " . $stmtInsert->error;
			exit;
		}
		
		echo "Score Submitted";
	}
	$mysqli->close();
?>