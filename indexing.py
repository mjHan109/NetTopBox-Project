import os
import re
import chardet
import json
import time
import datetime
import pandas as pd
from elasticsearch import AuthenticationException
from elasticsearch import Elasticsearch
from elasticsearch.helpers import bulk


def config_reading():
    current_directory = os.getcwd()
    # print(current_directory)
    config_file = os.path.join(current_directory, 'config.json')
    # print(config_file)
    if os.path.isfile(config_file):
        with open(config_file, 'r', encoding='utf-8') as file:
            json_data = json.load(file)
            return json_data
        
    else:
        print("config.json 파일을 찾을 수 없습니다.")
        return None
    


def indexing_csv(root_path, sub_path, csv_path, elastic_host, elastic_port, elastic_user):
    try:
        es = Elasticsearch(
            [f"http://{elastic_host}:{elastic_port}"],
            http_auth=(elastic_user, "Nettars09so"),
            verify_certs=False,
            ssl_show_warn=False,
            max_retries=10,
            retry_on_timeout=True,
            ignore_compatibility=True
        )
    except AuthenticationException as e:
        print(f"Authentication failed: {e}")

    output_data = []
    result_file_path = os.path.join(root_path, "indexing_result.txt")

    if not os.path.exists(csv_path):
        print("경로가 존재하지 않습니다. 사전 분석을 먼저 진행 해주세요.\n")
    else:
        for file_name in os.listdir(csv_path):
            if file_name.endswith('.csv'):
                    file_path = os.path.join(csv_path, file_name)

                    file_extension = os.path.splitext(file_name)[1]
                    file_name = os.path.splitext(file_name)[0]
                    if "(C_)" in file_name:
                        file_name = file_name.rsplit("(", 1)[0].strip()
                    if "08" in file_name:
                        file_name = "08_Wifi Info"
                    file_creation_time = datetime.datetime.fromtimestamp(os.path.getctime(file_path)).strftime('%Y/%m/%d %H:%M:%S')
                    file_modified_time = datetime.datetime.fromtimestamp(os.path.getmtime(file_path)).strftime('%Y/%m/%d %H:%M:%S')
                    indexing_time = datetime.datetime.now().strftime('%Y/%m/%d %H:%M:%S')
                    keyword = file_name.split("_", 1)[-1]
                    
                    with open(file_path, 'rb') as file:
                        file_contents = file.read()
                        encoding = chardet.detect(file_contents)['encoding']
                        file_contents = file_contents.decode(encoding)
                        file_contents = file_contents.replace('"', '')
                        file_contents = re.sub(r'\t', '\\t', file_contents)
                        file_contents = re.sub(r'\r', '\\r', file_contents)
                        file_contents = re.sub(r'\n', '\\n', file_contents)
                    
                    data = {
                        '확장자': file_extension,
                        '파일명': file_name,
                        '만든 날짜': file_creation_time,
                        '수정 날짜': file_modified_time,
                        "인덱싱 시간" : indexing_time,
                        '서비스 명': 'Footprinting',
                        'keyword' : keyword,
                        '절대 경로': root_path,
                        '원본 파일 경로': csv_path,
                        'contents': file_contents
                    }
                    output_data.append(data)

        df = pd.DataFrame(output_data)

        index_name = "result_analysis_info"

        if es.indices.exists(index=index_name):
            print("Index already exists. Deleting the existing index...\n")
            es.indices.delete(index=index_name)

        actions = [
            {
                "_index": index_name,
                "_source": record
            }
            for record in df.to_dict(orient='records')
            ]
        
        for idx, action in enumerate(actions, start=1):
            progress = idx / len(actions) * 100
            print(f"인덱싱 진행 : {round(progress)}%\n")

        
        bulk(es, actions)

        print(f"총 {len(actions)} 건 중 {len(actions)} 인덱싱 완료\n")
        print("인덱싱이 완료되었습니다.")
    

    
def main():
    start_time = time.time()
    json_data = config_reading()
    root_path = json_data['root_path']
    sub_path = json_data['sub_path']
    csv_export = os.path.join(root_path, sub_path['CSV_Export'])
    elastic_host = json_data['elastic_host']
    elastic_port = json_data['elastic_port']
    elastic_user = json_data['elastic_username']

    indexing_csv(root_path, sub_path, csv_export, elastic_host, elastic_port, elastic_user)
    end_time = time.time()

    print("업로드 걸린 시간(초): %.2f seconds" % (end_time - start_time), "\n")
if __name__ == "__main__":
    main()