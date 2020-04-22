<?php

	$connection = mysqli_connect('localhost', 'root', '', 'unity');

	// check that conection happened
	if (mysqli_connect_errno()) {
		echo("1"); // error code #1 - connection failed
		exit();
	}

	// Uncomment for avoiding SQL injection
	/*$username = mysqli_real_escape_string($connection, $_POST["name"]);
	$usernameclean = filter_var($username, FILTER_SANITIZE_STRING, FILTER_FLAG_STRIP_LOW | FILTER_FLAG_STRIP_HIGH);*/
	$username = $_POST["username"];
	$password = $_POST["password"];

	// check if name exists
	$nameCheckQuery = "SELECT username, password, score FROM php WHERE username='" . $username . "';";
	$nameCheck = mysqli_query($connection, $nameCheckQuery) or die("2"); // error code #2 - name check query failed

	if (mysqli_num_rows($nameCheck) != 1)
	{
		echo "5: Either no user with name " . $username . " or more than one"; // Error code #5 - number of names matching =! 1
		exit();
	}

	// Get Login info from query
	$existingInfo = mysqli_fetch_assoc($nameCheck);
	$registeredPwd = $existingInfo["password"];

	if ($registeredPwd != $password) {
		echo "6: Incorrect password."; // error code 6 - pwd does not hash to match table
		exit();
	}

	echo "0\t" . $existingInfo["score"];

?>