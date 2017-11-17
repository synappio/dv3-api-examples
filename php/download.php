<?php
  require_once 'dv3_api.php';
  define('API_KEY', $argv[1]);
  $result_fn = download_result($argv[2], false);
  print("Result downloaded to $result_fn\n");
