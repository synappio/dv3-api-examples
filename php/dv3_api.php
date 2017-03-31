<?php
  define('API_ROOT', 'https://dv3.datavalidation.com/api/v2');

  function _get_dv3_api_curl_handle($url) {
    $ch = curl_init();
    curl_setopt($ch, CURLOPT_URL, $url);
    curl_setopt($ch, CURLOPT_RETURNTRANSFER, true);
    curl_setopt($ch, CURLOPT_HTTPHEADER, ['Authorization: bearer ' . API_KEY]);
    return $ch;
  }

  function _get_dv3_api_parsed_result($ch) {
    $output = curl_exec($ch);
    $status_code = curl_getinfo($ch, CURLINFO_HTTP_CODE);
    curl_close($ch);
    assert($status_code == 200);
    return json_decode($output);
  }

  function realtime_check($email) {
    $ch = _get_dv3_api_curl_handle(API_ROOT . '/realtime/?email=' . urlencode($email));
    $result = _get_dv3_api_parsed_result($ch);
    assert($result->{'status'} == 'ok');
    return $result->{'grade'};
  }

  function get_upload_url($list_name, $email_column_index, $first_line_is_header) {
    assert(is_numeric($email_column_index));
    assert(is_bool($first_line_is_header));
    $url = API_ROOT . '/user/me/list/create_upload_url/?name=' . urlencode($list_name)
      . '&email_column_index=' . $email_column_index . '&has_header=' . $first_line_is_header;
    $ch = _get_dv3_api_curl_handle($url);
    return _get_dv3_api_parsed_result($ch);
  }

  if (!function_exists('curl_file_create')) {
    // compatibility with PHP < 5.5
    function curl_file_create($filename, $mimetype = '', $postname = '') {
      return "@$filename;filename="
        . ($postname ?: basename($filename)) . ($mimetype ? ";type=$mimetype" : '');
    }
  }

  function upload($upload_url, $file_name) {
    $ch = _get_dv3_api_curl_handle($upload_url);
    curl_setopt_array($ch, array(
      CURLOPT_POST => true,
      CURLOPT_POSTFIELDS => array(file => curl_file_create($file_name)),
    ));
    return _get_dv3_api_parsed_result($ch);
  }

  function get_list_info($list_id) {
    $ch = _get_dv3_api_curl_handle(API_ROOT . '/user/me/list/' . urlencode($list_id) . '/');
    return _get_dv3_api_parsed_result($ch);
  }
