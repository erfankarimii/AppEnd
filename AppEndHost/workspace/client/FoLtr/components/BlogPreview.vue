<template>
    <section id="blog-section" class="section bg-white">
        <div class="container">
            <div class="d-flex justify-content-between align-items-center mb-5">
                <h2 class="section-title mb-0">{{ translate("Latest News") }}</h2>
                <a href="#" class="btn btn-outline-primary">{{ translate("View All") }}</a>
            </div>
            <div class="row g-4">
                <div class="col-md-24 col-lg-16" v-for="(post, index) in posts" :key="index">
                    <div class="card blog-card h-100">
                        <div class="blog-image">
                            <div class="blog-image-placeholder d-flex align-items-center justify-content-center">
                                <i :class="post.icon"></i>
                            </div>
                        </div>
                        <div class="card-body">
                            <small class="text-muted">{{ post.date }}</small>
                            <h5 class="card-title mt-2 mb-3">{{ translate(post.title) }}</h5>
                            <p class="card-text text-muted">{{ translate(post.excerpt) }}</p>
                            <a href="#" class="btn btn-link p-0">{{ translate("Read More") }} <i class="fa-solid fa-arrow-right ms-1"></i></a>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>
</template>

<script>
let _this = { 
    cid: "", 
    c: null,
    posts: [
        {
            icon: "fa-solid fa-code fa-4x text-primary opacity-50",
            date: "2024/04/04",
            title: "Getting Started with AppEnd",
            excerpt: "Learn how to build your first application using AppEnd low-code platform in just a few minutes."
        },
        {
            icon: "fa-solid fa-rocket fa-4x text-primary opacity-50",
            date: "2024/03/30",
            title: "Best Practices for Low-Code Development",
            excerpt: "Discover the best practices and tips for developing applications faster with low-code tools."
        },
        {
            icon: "fa-solid fa-lightbulb fa-4x text-primary opacity-50",
            date: "2024/03/25",
            title: "New Features in AppEnd 2.0",
            excerpt: "Explore the latest features and improvements in AppEnd version 2.0 that make development even easier."
        }
    ]
};

export default {
    setup(props) { 
        _this.cid = props['cid']; 
    },
    data() { 
        return _this; 
    },
    created() { 
        _this.c = this; 
    },
    mounted() { },
    methods: {
        translate(key) {
            if (typeof shared !== 'undefined' && typeof shared.translate === 'function') {
                return shared.translate(key);
            }
            return key;
        }
    },
    props: { cid: String }
}
</script>

<style scoped>
.blog-card {
    border: none;
    border-radius: 15px;
    overflow: hidden;
    transition: transform 0.3s ease, box-shadow 0.3s ease;
}

.blog-card:hover {
    transform: translateY(-5px);
    box-shadow: 0 10px 25px rgba(0,0,0,0.1);
}

.blog-image {
    height: 200px;
    overflow: hidden;
}

.blog-image-placeholder {
    height: 100%;
    background: linear-gradient(135deg, #f5f7fa 0%, #c3cfe2 100%);
}

.blog-card .btn-link {
    text-decoration: none;
    color: var(--fo-primary);
    font-weight: 600;
}

.blog-card .btn-link:hover {
    text-decoration: underline;
}
</style>


