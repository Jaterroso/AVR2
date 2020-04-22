<?php

	$connection = mysqli_connect('localhost', 'root', '', 'unity');

	// check that conection happened
	if (mysqli_connect_errno()) {
		echo("1"); // error code #1 - connection failed
		exit();
	}

	$username = $_POST["username"];
	$newscore = $_POST["score"];

	// double check there is only one user with this name
	$nameCheckQuery = "SELECT username FROM php WHERE username='" . $username . "';";

	$nameCheck = mysqli_query($connection, $nameCheckQuery) or die("2: Name check query failed."); // error code #2 - name check query failed

	if (mysqli_num_rows($nameCheck) != 1)
	{
		echo "5: Either no user with name " . $username . " or more than one"; // Error code #5 - number of names matching =! 1
		exit();
	}

	$updateQuery = "UPDATE php SET score =" . $newscore . " WHERE username = '" . $username . "';";
	mysqli_query($connection, $updateQuery) or die("7: Save query failed."); // error code #7 - save query failed

	echo "0";

?>