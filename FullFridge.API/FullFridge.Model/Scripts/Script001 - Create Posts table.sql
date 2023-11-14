CREATE TABLE IF NOT EXISTS public.posts
(
    id uuid NOT NULL DEFAULT uuid_generate_v4(),
    title character varying COLLATE pg_catalog."default",
    content character varying COLLATE pg_catalog."default",
    created_by uuid,
    created_at timestamp with time zone DEFAULT CURRENT_TIMESTAMP,
    recipe_id uuid,
    CONSTRAINT posts_pkey PRIMARY KEY (id)
)

ALTER TABLE IF EXISTS public.posts
    OWNER to postgres;


CREATE TABLE public.post_comments
(
    id uuid DEFAULT uuid_generate_v4(),
    post_id uuid,
    content character varying,
    created_by uuid,
    created_at timestamp with time zone DEFAULT CURRENT_TIMESTAMP
);

ALTER TABLE IF EXISTS public.post_comments
    OWNER to postgres;