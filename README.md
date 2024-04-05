A RAG system for (Open)AI based question/answer sessions.

The RAG consists of files being uploaded (by the user), spilt into chunks of text that get stored in the (Postgresql) database. 
Also it is possible to enter snippets of wisdom and store them in the database. 
Both (chunks and snippets) can be presented to the embedding engine from OpenAI to get an embeddings which is also stored in the same database.

Finally a Chat can be created that combines prompts, user question and relevant chunks (based on cosine distance) in a message to the AI.
Which then answers, both question, answer and relevant costs/metrics are stored in the database.
