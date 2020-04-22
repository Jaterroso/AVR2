<?php

	$connection = mysqli_connect('localhost', 'root', '', 'unity');

	// check that conection happened
	if (mysqli_connect_errno()) {
		echo("1"); // error code #1 - connection failed
		exit();
	}

	$username = $_POST["username"];
	$password = $_POST["password"];

	// check if name exists
	$nameCheckQuery = "SELECT username FROM php WHERE username='" . $username . "';";
	$nameCheck = mysqli_query($connection, $nameCheckQuery) or die("2"); // error code #2 - name check query failed

	if (mysqli_num_rows($nameCheck) > 0) {
		echo "3: Name already exists."; // error code #3 - name exists cannot register
		exit(); 
	}

	// Add the user to the table
	$insertUserQuery = "INSERT INTO php (username, password) VALUES ('" . $username . "', '" . $password . "');";

	mysqli_query($connection, $insertUserQuery) or die("4: Insert player query failed."); // error code 4 - insert query failed

	echo("0");

?>