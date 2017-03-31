<?php
  require_once 'dv3_api.php';
  define('API_KEY', $argv[1]);

  $list_name = $argv[2];
  $file_name = $argv[3];
  assert(is_file($file_name));

  echo("Getting upload URL...\n");
  $upload_url = get_upload_url($list_name, 0, false);
  echo("Upload URL: $upload_url\n");
  echo("Uploading file contents\n");
  $list_id = upload($upload_url, $file_name);
  echo("List created. List id: $list_id\n");

  while (true) {
    $info = get_list_info($list_id);
    if ($info->{'status_value'} == 'VALIDATED') {
      echo("List validated. Grade summary: " . var_export($info->{'grade_summary'}, true) . "\n");
      break;
    } else if ($info->{'status_value'} == 'FAILED') {
      echo("List validation failed!\n");
      break;
    }

    echo("Status: {$info->{'status_value'}} ({$info->{'status_percent_complete'}} %)\n");
    sleep(5);
  }
