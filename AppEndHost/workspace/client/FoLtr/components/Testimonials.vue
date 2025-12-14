<template>
    <section id="testimonials-section" class="section bg-white">
        <div class="container">
            <h2 class="section-title">{{ translate("Testimonials") }}</h2>
            <div class="row g-4">
                <div class="col-md-24 col-lg-16" v-for="(testimonial, index) in testimonials" :key="index">
                    <div class="card testimonial-card h-100">
                        <div class="card-body text-center">
                            <div class="testimonial-avatar">
                                <i class="fa-solid fa-user"></i>
                            </div>
                            <p class="card-text mb-3">"{{ translate(testimonial.text) }}"</p>
                            <h6 class="card-title mb-1">{{ translate(testimonial.name) }}</h6>
                            <small class="text-muted">{{ translate(testimonial.role) }}</small>
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
    testimonials: [
        {
            name: "Developer 1",
            role: "Software Engineer",
            text: "AppEnd has revolutionized how we build applications. The low-code approach saves us weeks of development time."
        },
        {
            name: "Developer 2",
            role: "Full-Stack Developer",
            text: "The dynamic code compilation feature is amazing. We can make changes without restarting the server."
        },
        {
            name: "Developer 3",
            role: "Tech Lead",
            text: "Best low-code platform I've used. Clean architecture, easy to learn, and powerful features."
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
</style>

