<?php
  require_once 'dv3_api.php';
  define('API_KEY', $argv[1]);
  $grade = realtime_check($argv[2]);

  print("{$argv[2]} got a $grade.\n");
