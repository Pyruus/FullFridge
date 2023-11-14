CREATE TABLE public.comments (
    id uuid DEFAULT public.uuid_generate_v4() NOT NULL,
    content character varying,
    created_by_id uuid,
    created_at timestamp with time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    recipe_id uuid,
    rating double precision
);

ALTER TABLE public.comments OWNER TO postgres;


CREATE TABLE public.recipes (
    id uuid DEFAULT public.uuid_generate_v4() NOT NULL,
    title character varying,
    description character varying,
    created_by_id uuid,
    created_at timestamp with time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    image character varying,
    rating double precision,
    mealdb_id integer
);

ALTER TABLE public.recipes OWNER TO postgres;


CREATE TABLE public.recipes_products (
    recipe_id uuid,
    product_id integer
);

ALTER TABLE public.recipes_products OWNER TO postgres;


CREATE TABLE public.users (
    id uuid DEFAULT public.uuid_generate_v4() NOT NULL,
    email character varying,
    password character varying,
    role character varying,
    name character varying,
    surname character varying
);

ALTER TABLE public.users OWNER TO postgres;