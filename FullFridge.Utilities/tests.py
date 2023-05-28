import psycopg2
import time
import requests
import json
from config import config

def get_api_data(request_url):
    # Make a request to the API
    response = requests.get(request_url)  # Replace with the actual API endpoint

    # Check if the request was successful (status code 200)
    if response.status_code == 200:
        # Parse the JSON data from the response
        data = response.json()

        # Extract the values you're interested in from the data
        values = data['hints']  # Replace 'values' with the actual key in the API response

        return data
    else:
        # If the request was not successful, print the status code and raise an exception
        print("Error: API request failed with status code", response.status_code)
        response.raise_for_status()

def insert():
    """ Connect to the PostgreSQL database server """
    conn = None
    api_url = "https://api.edamam.com/api/food-database/v2/parser?app_id=eeb6bd23&app_key=%209b40881c82c2425a122df43de8ef1176&nutrition-type=cookin"
    try:
        # read connection parameters
        params = config()
        # connect to the PostgreSQL server
        print('Connecting to the PostgreSQL database...')
        conn = psycopg2.connect(**params)
        # create a cursor
        cur = conn.cursor()
        
        #insert new order in a loop
        query_insert = "INSERT INTO public.\"Products\" (\"Name\", \"Calories\") VALUES (%s, %s)"
        while(1):
            api_response = get_api_data(api_url)
            food = api_response['hints']
            links = api_response['_links']
            print("API response complete")
            i = 1
            for row in food:
                print(i)
                i+=1
                food_name = row['food']['label'].replace("'", "''")  # Replace single quotes with double single quotes
                cur.execute(query_insert, (food_name, row['food']['nutrients']['ENERC_KCAL']))
                conn.commit()
            
            api_url = links['next']['href']
            print("next call:" + api_url)
            time.sleep(2)
       
	# close the communication with the PostgreSQL
        cur.close()
    except (Exception, psycopg2.DatabaseError) as error:
        print(error)
    finally:
        if conn is not None:
            conn.close()
            print('Database connection closed.')

if __name__ == '__main__':
    insert()