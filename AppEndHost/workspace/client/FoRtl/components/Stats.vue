<template>
    <section id="stats-section" class="section stats-section">
        <div class="container">
            <div class="row g-4">
                <div class="col-24 col-12" v-for="(stat, index) in stats" :key="index">
                    <div class="stat-card text-center">
                        <div class="stat-icon mb-3">
                            <i :class="stat.icon"></i>
                        </div>
                        <h3 class="stat-number mb-2">{{ stat.value }}</h3>
                        <p class="stat-label">{{ translate(stat.label) }}</p>
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
    stats: [
        {
            icon: "fa-solid fa-fw fa-users fa-3x",
            value: "10K+",
            label: "Active Users"
        },
        {
            icon: "fa-solid fa-fw fa-folder-open fa-3x",
            value: "5K+",
            label: "Projects"
        },
        {
            icon: "fa-solid fa-fw fa-code fa-3x",
            value: "1M+",
            label: "Lines of Code"
        },
        {
            icon: "fa-solid fa-fw fa-heart fa-3x",
            value: "500+",
            label: "Happy Clients"
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
.stats-section {
    background: linear-gradient(135deg, var(--fo-gradient-start) 0%, var(--fo-gradient-end) 100%);
    color: white;
}

.stat-card {
    padding: 2rem 1rem;
}

.stat-icon {
    color: rgba(255, 255, 255, 0.9);
}

.stat-number {
    font-size: 3rem;
    font-weight: 700;
    color: white;
    margin: 0;
}

.stat-label {
    font-size: 1.1rem;
    color: rgba(255, 255, 255, 0.9);
    margin: 0;
}

@media (max-width: 768px) {
    .stat-number {
        font-size: 2rem;
    }
    
    .stat-icon i {
        font-size: 2rem !important;
    }
}
</style>


